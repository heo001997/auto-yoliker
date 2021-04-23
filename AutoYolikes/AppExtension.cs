using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoYolikes
{
    
    class AppExtension
    {
        public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.9358.63 Safari/537.36";
        public static int DieCookieCount;
        public static int LiveCookieCount;
        public static bool isstop;
        public static void AddCookie(DataGridView dtgrvdata)
        {
            try
            {
                DataObject o = (DataObject)Clipboard.GetDataObject();
                string[] pastedRows = Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                foreach (string pastedRow in pastedRows)
                {
                    if (!pastedRow.Replace("\t", "").Trim().Equals(""))
                    {
                        string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });
                        if (pastedRowCells.Length == 1)
                        {
                            int myRowIndex = dtgrvdata.Rows.Add();
                            dtgrvdata.Rows[myRowIndex].Cells["cldata"].Value = pastedRowCells[0];
                            dtgrvdata.Rows[myRowIndex].Cells["cllive"].Value = "nothing";
                            dtgrvdata.Rows[myRowIndex].Cells["clstt"].Value = myRowIndex.ToString();
                        }
                        else if (pastedRowCells.Length == 2)
                        {
                            int myRowIndex = dtgrvdata.Rows.Add();
                            for (int i = 0; i < 2; i++)
                            {
                                dtgrvdata.Rows[myRowIndex].Cells[i + 1].Value = pastedRowCells[i];
                            }
                        }
                        else if (pastedRowCells.Length >= 3)
                        {
                            int myRowIndex = dtgrvdata.Rows.Add();
                            dtgrvdata.Rows[myRowIndex].Cells[1].Value = pastedRowCells[0];
                            dtgrvdata.Rows[myRowIndex].Cells[2].Value = pastedRowCells[1];
                            dtgrvdata.Rows[myRowIndex].Cells[3].Value = pastedRowCells[2];

                        }
                    }
                }
            }
            catch { }
        }
        public static void StartCheckCookie(int row, DataGridView dtgrvdata)
        {
            //string cookie = dtgrvdata.Rows[row].Cells["cldata"].Value.ToString();
            //if (cookie.Contains("useragent="))
            //{
            //    cookie = Regex.Replace(cookie, "useragent=([a-zA-Z0-9]{100,300})", "");
            //    dtgrvdata.Rows[row].Cells["cldata"].Value = cookie;
            //}    
            //dtgrvdata.Rows[row].Cells["cllive"].Value = "Checking...";
            //RequestServiceXnet requestService = new RequestServiceXnet(cookie, UserAgent, "");
            //string response = requestService.RequestGet("https://mbasic.facebook.com/");
            //if (!response.Contains("xc_message"))
            //{
            //    DieCookieCount += 1;
            //    dtgrvdata.Rows[row].Cells["cllive"].Value = "Die";
            //    dtgrvdata.Rows[row].DefaultCellStyle.BackColor = Color.Red;
            //}    
            //else
            //{
            //    LiveCookieCount += 1;
            //    dtgrvdata.Rows[row].Cells["cllive"].Value = "Live";
            //    dtgrvdata.Rows[row].DefaultCellStyle.BackColor = Color.Green;
            //    //dtgrvdata.Rows[row].DefaultCellStyle.ForeColor = Color.White;
            //    //string Token = Regex.Match(response, "EAA[a-zA-Z0-9]{100,220}").Groups[0].Value;
            //    //dtgrvdata.Rows[row].Cells["cltoken"].Value = Token;
            //}
            //requestService.CloseRequest();
        }
        public static void CheckListCookie(DataGridView dtgrvdata)
        {
            int iThread = 0;
            int maxThread = 10;
            new Thread(() =>
            {
                for (int i = 0; i < dtgrvdata.Rows.Count -1;)
                {
                        if (iThread < maxThread)
                        {
                            Interlocked.Increment(ref iThread);
                            int row = i;
                            new Thread(() =>
                            {
                                StartCheckCookie(row, dtgrvdata);
                                Interlocked.Decrement(ref iThread);
                            }).Start();
                            i++;
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                }
            }).Start();
        }
        public static string RegexByNameHtml(string inputHtml, string name, int group)
        {
            return Regex.Match(inputHtml, $"name=\"{name}\" value=\"(.*?)\"").Groups[group].Value;
        }
        public static string tfacode(string code)
        {
            string result = "";
            xNet.HttpRequest http = new xNet.HttpRequest();
            try
            {
                code = code.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToString();
                string response = http.Get("http://2fa.live/tok/" + code).ToString();
                result = Regex.Match(response, "token.*?(\\d+)").Groups[1].Value;
            }
            catch
            {
                result = "";
            }
            finally
            {
                http.Dispose();
                http.Close();
            }
            return result;
        }
        public static String GetTimestamp()
        {
            DateTime value = new DateTime();
            return value.ToString("yyyyMMddHHmmssffff");
        }
        public static string Reslove2CaptchaCaptcha(string captchaKey, string ggKey, string url)
        {
            string capt = "";

            //_2Captcha reCapt = new _2Captcha(captchaKey);

            //bool isSuccess = reCapt.SolveRecaptchaV2(ggKey, url, out capt);

            //while (!isSuccess)
            //{
            //    if(isstop == true)
            //    {
            //        break;
            //    }    
            //    isSuccess = reCapt.SolveRecaptchaV2(ggKey, url, out capt);
            //    Thread.Sleep(TimeSpan.FromSeconds(2));
            //}

            return capt;
        }
        public static void Writelog(string message, TextBox txt, int NumberThread)
        {
            try
            {
                string datetimestr = DateTime.Now.ToString("HH-mm-ss");
                txt.Invoke((MethodInvoker)delegate ()
                {
                    txt.AppendText("[ " + NumberThread.ToString() + " ] " + datetimestr + " ==> " + message + "\r\n");
                });
            }
            catch (Exception ex)
            {
                File.AppendAllText("ErrorLog.txt", ex.ToString() + "\r\n");
            }
        }
        public static string GetSystemUid()
        {
            string text = @"SOFTWARE\Microsoft\Cryptography";
            string text2 = "MachineGuid";
            string result;
            using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey registryKey2 = registryKey.OpenSubKey(text))
                {
                    bool flag = registryKey2 == null;
                    if (flag)
                        throw new KeyNotFoundException(string.Format("Key Not Found: {0}", text));
                    object value = registryKey2.GetValue(text2);
                    bool flag2 = value == null;
                    if (flag2)
                        throw new IndexOutOfRangeException(string.Format("Index Not Found: {0}", text2));
                    string text3 = value.ToString();
                    result = text3;
                }
            }
            return result;
        }
        public static void LoadDatagridview(DataGridView dgv, string namePath)
        {
            try
            {
                List<string> list = File.ReadAllLines(namePath).ToList<string>();
                bool flag = list.Count > 0;
                if (flag)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        string row = list[i];
                        DataGridViewRowCollection rows = dgv.Rows;
                        object[] values = row.Split(new char[]{'|'});
                        rows.Add(values);
                    }
                    for (int j = 0; j < dgv.Rows.Count - 1; j++)
                    {
                        string dtgDieLive = dgv.Rows[j].Cells["cllive"].Value.ToString();
                        if (dtgDieLive == "Die")
                        {
                            dgv.Rows[j].DefaultCellStyle.BackColor = Color.Red;
                            dgv.Rows[j].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else
                        {
                            dgv.Rows[j].DefaultCellStyle.BackColor = Color.Green;
                            dgv.Rows[j].DefaultCellStyle.ForeColor = Color.White;
                        }
                    }
                }
            }
            catch { }
        }
        public  string Decrypt(string code)
        {
            code = code.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToString();
            System.Net.WebClient requestServiceXnet = new System.Net.WebClient();
            string response = requestServiceXnet.DownloadString("http://2fa.live/tok/" + code).ToString();
            return Regex.Match(response, "token.*?(\\d+)").Groups[1].Value;
        }
    }
}
