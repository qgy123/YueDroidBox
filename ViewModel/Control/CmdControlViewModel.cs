using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Stylet;
using YueDroidBox.Core.CmdHost;
using YueDroidBox.View.Control;

namespace YueDroidBox.ViewModel.Control
{
    public class CmdControlViewModel : Screen, ICmdReceiver
    {
        public string CmdText { get; set; } = "";

        public int CursorPosition
        {
            get; 
            set;
        }
        public string Dir { get; private set; } = "";
        public int DataLen { get;  private set; }
        public int SelectionStart { get;  private set; }
        public int SelectionLength { get;  private set; }
        public string SelectedText { get; set; }


        private int TabIndex { get; set; } = 0;

        private readonly HistoryCommand historyCommand;
        private readonly CmdReader cmdReader;

        public CmdControlViewModel()
        {
            this.DisplayName = "CMD Window";

            historyCommand = new HistoryCommand();
            cmdReader = new CmdReader();
            cmdReader.Register(this);

            Init();
        }

        private bool Init(string projectPath = null)
        {
            cmdReader.InitDir = projectPath;

            return cmdReader.Init();
        }

        public void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                cmdReader.SendCtrlC();
                e.Handled = true;
                return;
            }

            if (NoEditArea(e))
            {
                if (IsCharacterOrEnter(e.Key))
                {
                    FocusEnd();
                }
                else if (e.Key >= Key.Left && e.Key <= Key.Down)
                {
                    return;
                }
                else
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.Key == Key.Tab)
            {
                HandleTab();
                e.Handled = true;
                return;
            }
            else
            {
                ResetTabComplete();
            }

            if (IsControlKeys(e))
            {
                e.Handled = true;
            }
        }


        public void SetPath(string projectPath)
        {
            cmdReader.InitDir = projectPath;
        }

        public void SetShell(string shell)
        {
            cmdReader.Shell = shell;
        }

        public void InvokeCmd(string msg, string cmd = "")
        {
            if (string.IsNullOrEmpty(cmd))
            {
                return;
            }

            // Todo: need dispatcher?
            AppendOutput(msg);

            cmdReader.Input(cmd);
        }

        public void HandleTab()
        {
            try
            {
                string completedLine = CompleteInput(CmdText, TabIndex);
                SetInput(completedLine);
                TabIndex += 1;
            }
            catch (Exception)
            {
                ResetTabComplete();
            }
        }

        public void SetInput(string input)
        {
            if (input != null)
            {
                CmdText = CmdText.Substring(0, DataLen) + input;
                FocusEnd();
            }
        }

        public void FocusEnd()
        {
            // Todo:
            //Rst.Select(Rst.Text.Length, 0);
            //CursorPosition = CmdText.Length -1;
            //SelectionStart = CmdText.Length;
            //SelectionLength = 0;
            //((CmdControlView)View).FuckBox.Select(CmdText.Length, 0);

            CursorPosition = CmdText.Length;
        }

        public string CompleteInput(string input, int index)
        {
            string tabHit = ExtractFileName(input);
            string additionalPath = SeperatePath(ref tabHit);

            string tabName = GetFile(additionalPath, tabHit, index);

            return input.Substring(0, input.Length - tabHit.Length) + tabName;
        }

        public string GetFile(string additionalPath, string tabHit, int index)
        {
            var files = Directory.GetFileSystemEntries(Dir + "\\" + additionalPath, tabHit + "*");

            if (files.Length == 0)
            {
                return "";
            }

            if (index >= files.Length)
            {
                ResetTabComplete();
                index = 0;
            }

            string tabFile = files[index];
            string tabName = tabFile.Substring(tabFile.LastIndexOf('\\') + 1);

            return tabName;
        }

        public string SeperatePath(ref string tabHit)
        {
            string additionalPath = "";

            if (tabHit.LastIndexOf('\\') != -1)
            {
                additionalPath += tabHit.Substring(0, tabHit.LastIndexOf('\\'));
                tabHit = tabHit.Substring(tabHit.LastIndexOf('\\') + 1);
            }

            return additionalPath;
        }

        public string ExtractFileName(string input)
        {
            int pos = input.LastIndexOf('"');
            if (pos == -1)
            {
                pos = input.LastIndexOf(' ');
            }

            return input.Substring(pos + 1);
        }

        public void ExtractDir(string outputs)
        {
            const string regex = @"((?<=^PS )|^)\w:\\[^\x00-\x1F]*(?=>\s*$)";

            string lastLine = outputs.Substring(outputs.LastIndexOf('\n') + 1);
            string dir = Regex.Match(lastLine, regex, RegexOptions.Compiled).Value;

            if (!string.IsNullOrEmpty(dir))
            {
                Dir = dir;
            }
        }

        public void ResetTabComplete()
        {
            TabIndex = 0;
        }

        public void AppendOutput(string text)
        {
            CmdText += text;
            DataLen = CmdText.Length;
            FocusEnd();
        }

        private bool IsControlKeys(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    SetInput(historyCommand.SelectPreviuos());
                    break;

                case Key.Down:
                    SetInput(historyCommand.SelectNext());
                    break;

                case Key.Home:
                    FocusEnd();
                    break;

                case Key.Return:
                    RunCmd();
                    break;

                default:
                    return false;
            }

            return true;
        }

        private void RunCmd()
        {
            string cmd = GetCmd();

            if (cmd == @"cls")
            {
                //Todo:dispatcher
                Clear();

                cmdReader.Input("");
            }
            else
            {
                //No async, ensure is done
                //Todo:dispatcher

                RemoveInput();

                cmdReader.Input(cmd);
            }

            historyCommand.Add(cmd);
        }

        public void RemoveInput()
        {
            CmdText = CmdText.Substring(0, DataLen);
        }

        public void Clear()
        {
            CmdText = "";
            DataLen = 0;
        }

        public string GetCmd()
        {
            return CmdText.Substring(DataLen, CmdText.Length - DataLen);
        }

        private bool NoEditArea(KeyEventArgs e)
        {
            if (CursorPosition < DataLen)
            {
                Console.WriteLine("Can't edit!");
                return true;
            }

            if (e.Key == Key.Back && CursorPosition <= DataLen)
            {
                return true;
            }

            return false;
        }

        private bool IsCharacterOrEnter(Key key)
        {
            if (key < Key.Z && key > Key.D0)
            {
                return true;
            }
            else if (key == Key.Enter)
            {
                return true;
            }

            return false;
        }

        public void AddData(string output)
        {
            ExtractDir(output);
            ResetTabComplete();

            // Todo: dispatcher
            AppendOutput(output);
        }
    }
}