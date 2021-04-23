using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AutoYolikes.Properties;
using AutoYolikes.FB;
using AutoYolikes.YolikerApp;
using AutoYolikes.CaptchaService;
using Anticaptcha_example.Api;

namespace AutoYolikes
{
    public partial class Form1 : Form
    {
        bool Isstop;
        //string useragent = "";
        Random randomaction = new Random();
        Dictionary<int, string> DicAction = new Dictionary<int, string>();
        public static Dictionary<string, string> ListFbLoged = new Dictionary<string, string>();
        public static Dictionary<string, string> ListFbTokens = new Dictionary<string, string>();
        public static List<string> ListFbDie = new List<string>();
        int totalLikeRec_ = 0;
        bool _isrun;
        string user = "";
        string expdate = "";
        public TypeLogin typeLogin_;
        public CaptchaService captchaService;
        IniFile iniFile = new IniFile("inf.ini");
        public Form1(string user_, string expdate_)
        {
            user = user_;
            expdate = expdate_;
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            InitDicAction();
        }

        private void addCookieListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void checkLiveListCookieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppExtension.CheckListCookie(dtgrvdata);
            lbdie.Text = AppExtension.DieCookieCount.ToString();
            lblive.Text = AppExtension.LiveCookieCount.ToString();
        }
        public static double ConvertMinutesToMilliseconds(double minutes)
        {
            return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
        }
        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
        private void btnstart_Click(object sender, EventArgs e)
        {
            
            iniFile.Write("captcha-service", cbbCapchaType.SelectedItem.ToString());
            iniFile.Write("captcha-service-api-key", txtapikey.Text);
            if (txtapikey.Text.Length < 2)
            {
                MessageBox.Show("API key is null !", "api key đâu ba ~~");
                return;
            }
            if (txtpostid.Text.Length < 2)
            {
                MessageBox.Show("Id Post Is Null", "id post đâu ba ~~");
                return;
            }
            // AppExtension.CheckListCookie(dtgrvdata);
            btnstart.Invoke((MethodInvoker)delegate ()
            {
                btnstart.Enabled = false;
                btnstop.Enabled = true;
            });
            Settings.Default.apikey = txtapikey.Text;
            Settings.Default.Save();
            ApplicationStart();
        }
        private void ApplicationStart()
        {
            int maxThread = Convert.ToInt32(numthread.Value);
            int curThread = 0;
            int changeIpAfter = Convert.ToInt32(1);
            int curChangeIp = 0;
            Isstop = false;
            _isrun = true;
            bool isDoneAll = true;
            btnstart.Enabled = false;
            btnstop.Enabled = true;
            new Thread(() =>
            {
            int i = 0;
            while (i < dtgrvdata.Rows.Count - 1)
            {
                while (isDoneAll == false)
                {
                    Application.DoEvents();
                    Thread.Sleep(1000);
                    if (curThread <= 0)
                    {
                        Interlocked.Increment(ref curChangeIp);
                        if (curChangeIp >= changeIpAfter)
                        {
                            curChangeIp = 0;
                        }
                        isDoneAll = true;
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                }
                if (curThread < maxThread)
                {
                    if (Isstop)
                    {
                        break;
                    }
                    Interlocked.Increment(ref curThread);
                    int row = i;
                    new Thread(() =>
                    {
                        StartRun(row);
                        Interlocked.Decrement(ref curThread);
                    }).Start();
                    i += 1;
                }
                else
                {
                    isDoneAll = false;
                    Thread.Sleep(1000);
                }
                if (Isstop)
                {
                    break;
                }
            }
            while (curThread > 0)
            {
                Thread.Sleep(500);
                Application.DoEvents();
            }
                btnstart.Invoke((MethodInvoker)delegate ()
                {
                    AppExtension.Writelog("Toàn bộ luồng chạy đã kết thúc !", txtlog, 201);
                    btnstart.Enabled = true;
                    btnstop.Enabled = false;
                    _isrun = false;
                });
            }).Start();
        }
        private string GetUID(string data)
        {
            if(typeLogin_ == TypeLogin.COOKIE)
            {
                return Regex.Match(data, "c_user=(\\d+);").Groups[1].Value;
            }    
            else
            {
                return data.Split('|')[0];
            }
        }
        private void StartRun(int row)
        {
            //string cookie = "";
            FacebookStatics facebookStatics = new FacebookStatics();
            string[] arraydataAccount = null;
            YolikerLogs yolikerLogs = new YolikerLogs()
            {
                dtgrvdata = dtgrvdata,
                row = row
            };
            Session session = new Session();
            try
            {
                START_RUN:
                if (Isstop == true)
                {
                    yolikerLogs.Write("đã dừng chương trình !", YolikerLogs.TypeLog.NORMAL);
                    return;
                }
                string data_ = dtgrvdata.Rows[row].Cells["cldata"].Value.ToString();
                string uid = GetUID(data_);
                if(!ListFbDie.Contains(uid))
                {
                    if(ListFbLoged.ContainsKey(uid))
                    {
                        facebookStatics.IssCookie = true;
                        facebookStatics.cookie = ListFbLoged[uid];
                        facebookStatics.access_token = ListFbTokens[uid];
                    } 
                    else
                    {
                        if(typeLogin_ == TypeLogin.COOKIE)
                        {
                            facebookStatics.IssCookie = true;
                            facebookStatics.cookie = data_;
                        }    
                        else
                        {
                            arraydataAccount = data_.Split('|');
                            facebookStatics.IssCookie = false;
                            facebookStatics.uid = uid;
                            facebookStatics.pass = arraydataAccount[1];
                            if (arraydataAccount.Count() >= 3)
                            {
                                facebookStatics.tfa = arraydataAccount[2];
                            }
                        }
                    }
                    if(ListFbTokens.ContainsKey(uid))
                    {
                        //session = new Session();
                        //session.CreateSession();
                        goto strunn;
                    }    
                    yolikerLogs.Write("đang login facebook...", YolikerLogs.TypeLog.NORMAL);
                    FacebookLogin login = new FacebookLogin(facebookStatics);
                    FacebookStatics.LoginStatus loginStatus = login.Login();
                    session = login.FacebookSession;
                    if(loginStatus != FacebookStatics.LoginStatus.SUCCESS)
                    {
                        yolikerLogs.Write(ErrorTypeHelper.GetDescriptionFacebookState(loginStatus), YolikerLogs.TypeLog.WARNING);
                        ListFbDie.Add(uid);
                        return;
                    }
                    strunn:;
                    if (!ListFbLoged.ContainsKey(uid))
                    {
                        ListFbLoged.Add(uid, facebookStatics.cookie);
                    }
                    if (!ListFbTokens.ContainsKey(uid))
                    {
                        ListFbTokens.Add(uid, facebookStatics.access_token);
                    }
                    CaptchaStatics.CaptchaService capService = (CaptchaStatics.CaptchaService)Enum.ToObject(typeof(CaptchaStatics.CaptchaService), cbbCapchaType.SelectedIndex);
                    CaptchaStatics captchaStatics = new CaptchaStatics()
                    {
                        Apikey = txtapikey.Text,
                        Url = YolikerStatics.LikeCustomUri
                    };
                    YolikerStatics yolikerStatics = new YolikerStatics()
                    {
                        captchaSvName = capService,
                        IdPost = txtpostid.Text,
                        TypeAction = GetAction()
                    };
                    Actions actions = new Actions()
                    {
                        statics = facebookStatics,
                        fbSession = session,
                        captchaSt = captchaStatics,
                        yolikerStatics = yolikerStatics,
                        Logs = yolikerLogs
                    };
                    yolikerLogs.Write("đang login yoliker...", YolikerLogs.TypeLog.NORMAL);
                    YolikerStatics.YolikerState yolikerState = actions.LoginYoliker();
                    if (yolikerState == YolikerStatics.YolikerState.SUCCESS)
                    {
                        yolikerState = actions.StartAction();
                        //session.Dispose();
                        //facebookStatics.dispose();
                        if (yolikerState == YolikerStatics.YolikerState.SUCCESS)
                        {
                            //yolikerLogs.Write("Tăng " + yolikerStatics.TypeAction + " thành công! + " + yolikerStatics.NumLikeDelivered.ToString() + " " + yolikerStatics.TypeAction, YolikerLogs.TypeLog.NORMAL);
                            AppExtension.Writelog("Tăng " + yolikerStatics.TypeAction + " thành công! + " + actions.NumLikeDelivered.ToString() + " " + yolikerStatics.TypeAction, txtlog, row);
                            totalLikeRec_ += actions.NumLikeDelivered;
                            lbtotallike.Text = totalLikeRec_.ToString();
                            if (chkautostop.Checked)
                            {
                                if (totalLikeRec_ >= Convert.ToInt32(numLike.Value))
                                {
                                    AppExtension.Writelog("Thread " + row.ToString() + " đã đủ số like theo yêu cầu !!! " + numLike.Value.ToString() + " LIKE", txtlog, row);
                                    yolikerLogs.Write("đã đủ số like --> tự động dừng tool !", YolikerLogs.TypeLog.NORMAL);
                                    Isstop = true;
                                    AppExtension.isstop = true;
                                    _isrun = false;
                                }
                            }
                            new Thread(() =>
                            {
                                checkapi();
                            }).Start();
                        }
                    }
                    else
                    {
                        yolikerLogs.Write(ErrorTypeHelper.GetDescriptionYolikerState(yolikerState), YolikerLogs.TypeLog.WARNING);
                    }
                    for (int i = 1; i < 16; i++)
                    {
                        if (Isstop == true)
                        {
                            yolikerLogs.Write("đã dừng chương trình !", YolikerLogs.TypeLog.NORMAL);
                            return;
                        }
                        yolikerLogs.Write("đang nghỉ " + i.ToString() + " phút / 16 phút", YolikerLogs.TypeLog.NORMAL);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }
                    goto START_RUN;
                }
                {
                    yolikerLogs.Write("fb này đã die !", YolikerLogs.TypeLog.WARNING);
                    return;
                }
            }
            catch(Exception ex)
            {
                File.AppendAllText("ErrorLog.txt", ex.ToString() + "\r\n");
            }
            finally
            {
                session.Dispose();
                facebookStatics.dispose();
                AppExtension.Writelog("Thread " + row.ToString() + " đã xong !!! ", txtlog, row);
            }
        }
        private string GetAction()
        {
            if(chkrandom.Checked)
            {
                int rdnum = randomaction.Next(1, 6);
                string action_ = DicAction[rdnum];
                return action_;
            }    
            if (like.Checked)
            {
                return DicAction[1];
            }
            if (haha.Checked)
            {
                return DicAction[2];
            }
            if (wow.Checked)
            {
                return DicAction[3];
            }
            if (love.Checked)
            {
                return DicAction[4];
            }
            if (angry.Checked)
            {
                return DicAction[5];
            }
            if (sad.Checked)
            {
                return DicAction[6];
            }
            return DicAction[1];
        }
        private void InitDicAction()
        {
            
            DicAction.Add(1, "LIKE");
            DicAction.Add(2, "HAHA");
            DicAction.Add(3, "WOW");
            DicAction.Add(4, "LOVE");
            DicAction.Add(5, "ANGRY");
            DicAction.Add(6, "SAD");
        }
        private void btnstop_Click(object sender, EventArgs e)
        {  
            try
            {
                AppExtension.Writelog("hãy chờ 1 chút để toàn bộ các luồng kết thúc !", txtlog, 201);
                Isstop = true;
                _isrun = false;
                AppExtension.isstop = true;
                //btnstart.Invoke((MethodInvoker)delegate ()
                //{
                //    btnstart.Enabled = true;
                //    btnstop.Enabled = false;
                //});
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_isrun == true)
            {
                if (MessageBox.Show("tool đang chạy, bạn có chắc chắn muốn thoát không ???", "Exit Confirm?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    goto exit_point;
                }
                else
                {
                    return;
                }
            }
            exit_point:;
            try
            {
                //StringBuilder stringBuilder = new StringBuilder();
                //for (int i = 0; i < dtgrvdata.Rows.Count - 1; i++)
                //{
                //    string datagridItemCookie = dtgrvdata.Rows[i].Cells["cldata"].Value.ToString();
                //    string datagridItemStt = dtgrvdata.Rows[i].Cells["clstt"].Value.ToString();
                //    string datagridItemLivedie = dtgrvdata.Rows[i].Cells["cllive"].Value.ToString();
                //    string datagridItem = datagridItemStt + "|" + datagridItemCookie + "|" + datagridItemLivedie;
                //    stringBuilder.AppendLine(datagridItem);
                //}
                //File.WriteAllText("data.txt", stringBuilder.ToString());
                //foreach (var process in Process.GetProcessesByName("chromedriver"))
                //{
                //    process.Kill();
                //}
                base.Dispose();
                Environment.Exit(0);
            }
            catch
            {
                base.Dispose();
                this.Close();
            }
        }

        private void chkfakeug_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //AppExtension.LoadDatagridview(dtgrvdata, "data.txt");
            //YolikersApp.Url = "https://app.mp3songs.desi/like_rs.php?type=custom"; //https://app.mp3songs.desi/likers.php?type=custom
            //YolikersApp.urllogin = "https://app.mp3songs.desi/login.php?access_token=";
            //YolikersApp.TokenFacebookAuthUrl = "https://m.facebook.com/composer/ocelot/async_loader/?publisher=feed";
            //FacebookAPI.accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            //FacebookAPI._content_type = "application/x-www-form-urlencoded";
            //FacebookAPI._mbasic_server_uri = "https://mbasic.facebook.com";
            //FacebookAPI._mbasic_checkpoint_uri = "https://mbasic.facebook.com/login/checkpoint/";
            YolikerStatics.LikeCustomUri = "https://app.mp3songs.desi/like_rs.php?type=custom";
            YolikerStatics.LoginUrl = "https://app.mp3songs.desi/login.php?access_token=";
            FacebookStatics.AndroidLoginAuthUrl = "https://m.facebook.com/v7.0/dialog/oauth?client_id=174829003346&redirect_uri=fbconnect://success&response_type=token&sdk=android-5.1.0";
            FacebookStatics.OAUTH_CONFIRM_URL = "https://m.facebook.com/v7.0/dialog/oauth/read/";
            cbbtypelogin.SelectedIndex = 0;
            lbacc.Text = $"{user} [ {expdate} ]";
            if (!File.Exists("xNet.dll"))
            {
                MessageBox.Show("bạn không có quyền truy cập vào tool này ! Error : Lib Xnet.dll Not Found", "Access Denied !");
                this.Close();
            }
            try
            {
                txtapikey.Text = iniFile.Read("captcha-service-api-key");
                cbbCapchaType.SelectedItem = iniFile.Read("captcha-service");
            }
            catch { }
        }

        private void checkBalanceAPI2captchaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtapikey.Text.Length < 2)
            {
                MessageBox.Show("API Key is null ! Please Add it and try Again !", "API đâu ba ~~");
                return;
            }
            lbbalan.Text = "checking...";
            new Thread(() =>
            {
                checkapi();
            }).Start();
        }

        private void seclectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dtgrvdata.SelectedRows)
                {
                    dtgrvdata.Rows.RemoveAt(row.Index);
                }
                lbtotaldata.Text = dtgrvdata.Rows.Count.ToString();
            }
            catch { }
        }

        private void allListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dtgrvdata.Rows.Clear();
                lbtotaldata.Text = dtgrvdata.Rows.Count.ToString();
            }
            catch { }
        }

        private void cookieDieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dtgrvdata.Rows.Count - 1; i++)
                {
                    string statusdie = dtgrvdata.Rows[i].Cells["cllive"].Value.ToString();
                    if(statusdie == "Die")
                    {
                        dtgrvdata.Rows.RemoveAt(i);
                    }    
                }
                lbtotaldata.Text = dtgrvdata.Rows.Count.ToString();
            }
            catch { }
        }

        private void btnadd_data_Click(object sender, EventArgs e)
        {
            cbbtypelogin.SelectedIndex = 1;
            AppExtension.AddCookie(dtgrvdata);
        }

        private void btnaddcookie_Click(object sender, EventArgs e)
        {
            cbbtypelogin.SelectedIndex = 0;
            AppExtension.DieCookieCount = 0;
            AppExtension.LiveCookieCount = 0;
            AppExtension.AddCookie(dtgrvdata);
            lbtotaldata.Text = dtgrvdata.Rows.Count.ToString();
            //AppExtension.CheckListCookie(dtgrvdata);
            //lbdie.Text = AppExtension.DieCookieCount.ToString();
            //lblive.Text = AppExtension.LiveCookieCount.ToString();
        }

        private void chkautostop_CheckedChanged(object sender, EventArgs e)
        {
            numLike.Enabled = chkautostop.Checked;
        }

        private void txtapikey_TextChanged(object sender, EventArgs e)
        {
            lbbalan.Text = "checking...";
            try
            {
                new Thread(() =>
                {
                    checkapi();
                }).Start();
            }
            catch { }
        }
        private void checkapi()
        {
            string resultBalance = "";
            try
            {
                if (captchaService == CaptchaService._2captcha)
                {
                    _2Captcha _2Captcha = new _2Captcha(txtapikey.Text);
                    resultBalance = _2Captcha.CheckBalance();
                }
                else if (captchaService == CaptchaService.capmonter)
                {
                    resultBalance = new CapMonsterCloud.CapMonsterClient(txtapikey.Text).GetBalanceAsync().Result.ToString();
                }
                else
                {
                    var api = new ImageToText
                    {
                        ClientKey = txtapikey.Text
                    };

                    var balance = api.GetBalance();

                    if (balance == null)
                        //DebugHelper.Out("GetBalance() failed. " + api.ErrorMessage, DebugHelper.Type.Error);
                        resultBalance = "ERROR_KEY";
                    else
                        resultBalance = balance.ToString();
                }
            }
            catch(Exception ex)
            {

                File.AppendAllText("error_main_form.txt", ex.ToString() + "\r\n");
                resultBalance = ex.Message;
                //MessageBox.Show(ex.Message + "\r\n" + ex.ToString());
            }
            finally
            {
                lbbalan.Text = resultBalance + " $";
            }
        }

        private void cbbtypelogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            TypeLogin TypeLog = (TypeLogin)Enum.ToObject(typeof(TypeLogin), cbbtypelogin.SelectedIndex);
            typeLogin_ = TypeLog;
        }
        public enum TypeLogin
        {
            COOKIE = 0,
            UID_PASS_TFA = 1,
            UID_PASS = 2
        }
        public enum CaptchaService
        {
            _2captcha = 0,
            capmonter = 1,
            anti_captcha = 2
        }

        private void cbbCapchaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaptchaService captchaServicename = (CaptchaService)Enum.ToObject(typeof(CaptchaService), cbbCapchaType.SelectedIndex);
            captchaService = captchaServicename;
            try
            {
                lbbalan.Text = "checking...";
                new Thread(() =>
                {
                    checkapi();
                }).Start();
            }
            catch { }
        }

        private void txtlog_TextChanged(object sender, EventArgs e)
        {
            if(txtlog.Lines.Length >= 1000)
            {
                txtlog.Clear();
            }    
        }

        private void lbrates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("-------------------\r\n 2CAPTCHA : 1000 captchas = 2,99$\r\nCapmonter : 1000 captchas = 0,6$\r\n Anti-captcha : 1000 captchas = 2$", "Captcha services rates", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
