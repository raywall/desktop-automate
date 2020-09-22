using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace DesktopAutomateConsoleApp
{
    public class DesktopCalculatorAutomate
    {
        #region Properties
        public Application application { get; set; }

        private Window window { get; set; }

        private Dictionary<char, string> buttons { get; set; }
        #endregion

        #region Events
        public delegate void ErrorOcurredHandler(Exception ex);
        public event ErrorOcurredHandler OnError;
        #endregion

        #region Methods for automate
        public bool OpenApplication(string path)
        {
            buttons = new Dictionary<char, string>
            {
                { '0', "124" },
                { '1', "125" },
                { '2', "126" },
                { '3', "127" },
                { '4', "128" },
                { '5', "129" },
                { '6', "130" },
                { '7', "131" },
                { '8', "132" },
                { '9', "133" },
                { '.', "85" },
                { ',', "85" },
                { '/', "90" },
                { 'x', "91" },
                { 'X', "91" },
                { '-', "93" },
                { '+', "92" },
                { '=', "112" },
            };

            try
            {
                if (string.IsNullOrEmpty(path))
                    throw new Exception("É necessário informar o caminho do aplicativo!");

                else if (!File.Exists(path))
                    throw new Exception("Não foi possível localizar o arquivo informado!");

                application = Application.Launch(path);
                application.WaitWhileBusy();

                Retorno:
                Thread.Sleep(1000);

                window = application.GetWindow(SearchCriteria.ByClassName("SciCalc"), InitializeOption.NoCache);

                if (window == null)
                    goto Retorno;
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }

            return true;
        }

        public bool CloseApplication()
        {
            try
            {
                if (window == null)
                    throw new Exception("Não foi possível localizar a janela da aplicação!");

                application.Close();
            }
            catch
            {
                OnError?.Invoke(new Exception("Ocorreu um erro ao tentar encerrar a aplicação!"));
            }

            return true;
        } 

        public bool PressButton(string expression)
        {
            try
            {
                if (string.IsNullOrEmpty(expression))
                    throw new Exception("Informe o bottão que deseja pressionar ou expressão que deseja digitar!");

                if (window == null)
                    throw new Exception("Não foi possível localizar a janela da aplicação!");

                else
                    foreach (char _button in expression.ToCharArray())
                    {
                        var automationId = buttons.Where(w => w.Key.Equals(_button)).FirstOrDefault().Value;

                        if (string.IsNullOrEmpty(automationId))
                            throw new Exception($"Não foi possível localizar o botão {_button}");

                        else
                        {
                            var button = window.Get<Button>(SearchCriteria.ByAutomationId(automationId));

                            if (button == null)
                                throw new Exception($"Não foi possivel localizar o botão {_button}");

                            else
                                button.Click();
                        }
                    }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }

            return true;
        }
        #endregion

        public DesktopCalculatorAutomate() { }

        public DesktopCalculatorAutomate(string path)
        {
            try
            {
                OpenApplication(path);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
        }

        public DesktopCalculatorAutomate(string path, string expression)
        {
            try
            {
                OpenApplication(path);

            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
            }
        }
    }
}
