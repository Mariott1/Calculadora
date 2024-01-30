using System.Data;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq.Expressions;

namespace Calculadora
{
    public partial class Form1 : Form
    {
        private string expressao = "";
        private bool result = false;

        public Form1()
        {
            InitializeComponent();
            txtValoresSecundarios.Text = "";
            txtValoresPrimarios.ReadOnly = true;
            txtValoresPrimarios.HideSelection = true;
            txtValoresSecundarios.ReadOnly = true;
            txtValoresSecundarios.HideSelection = true;

            this.Paint += new PaintEventHandler(degradeForm);


            //Numericos
            one.Click += (sender, e) =>
            {
                defineValores("1");
                equal.Focus();
            };
            two.Click += (sender, e) =>
            {
                defineValores("2");
                equal.Focus();
            };
            three.Click += (sender, e) =>
            {
                defineValores("3");
                equal.Focus();
            };
            four.Click += (sender, e) =>
            {
                defineValores("4");
                equal.Focus();
            };
            five.Click += (sender, e) =>
            {
                defineValores("5");
                equal.Focus();
            };
            six.Click += (sender, e) =>
            {
                defineValores("6");
                equal.Focus();
            };
            seven.Click += (sender, e) =>
            {
                defineValores("7");
                equal.Focus();
            };
            eigth.Click += (sender, e) =>
            {
                defineValores("8");
                equal.Focus();
            };
            nine.Click += (sender, e) =>
            {
                defineValores("9");
                equal.Focus();
            };
            zero.Click += (sender, e) =>
            {
                defineValores("0");
                equal.Focus();
            };
            //Virgula
            comma.Click += (sender, e) =>
            {
                trataVirgula(",");
                equal.Focus();
            };
            //Expressões
            mais.Click += (sender, e) =>
            {
                defineOperador("+");
                equal.Focus();
            };

            menos.Click += (sender, e) =>
            {
                defineOperador("-");
                equal.Focus();
            };

            vezes.Click += (sender, e) =>
            {
                defineOperador("*");
                equal.Focus();
            };

            divisao.Click += (sender, e) =>
            {
                defineOperador("/");
                equal.Focus();
            };

            perc.Click += (sender, e) =>
            {
                definePerc("%");
                equal.Focus();
            };
            raizQuadrada.Click += (sender, e) =>
            {
                defineRaizQuadrada("√");
                equal.Focus();
            };
            //Igual
            equal.Click += (sender, e) => calculaResultado();
            //Troca para + e - || * e /
            maisMenos.Click += (sender, e) =>
            {
                string[] partes = expressao.Split(new char[] { '+', '-', '*', '/' });

                if (partes.Length > 1)
                {
                    string firstNumber = partes.First();
                    string lastNumber = partes.Last();

                    // Encontra o índice do último operador na expressão
                    int ultimoOperadorIndex = expressao.LastIndexOfAny(new char[] { '+', '-', '*', '/' });

                    if (ultimoOperadorIndex >= 0)
                    {
                        // Obtém o operador antes do lastNumber
                        string operador = expressao.Substring(ultimoOperadorIndex, 1);

                        // Troca o operador
                        if (operador == "+")
                        {
                            operador = "-";
                        }
                        else if (operador == "-")
                        {
                            operador = "+";
                        }
                        else if (operador == "*")
                        {
                            operador = "/";
                        }
                        else if (operador == "/")
                        {
                            operador = "*";
                        }

                        expressao = expressao.Substring(0, ultimoOperadorIndex) + operador + lastNumber;
                    }
                    else
                    {
                        // Adiciona ou remove o sinal negativo no início da expressão
                        if (expressao.Length > 0 && expressao[0] == '-')
                        {
                            expressao = expressao = expressao.Substring(1);
                        }
                        else
                        {
                            expressao = "-" + lastNumber;
                        }
                    }

                }
                else
                {
                    // Adiciona ou remove o sinal negativo no início da expressão
                    if (expressao.Length > 0 && expressao[0] == '-')
                    {
                        expressao = expressao.Substring(1);
                    }
                    else
                    {
                        expressao = "-" + expressao;
                    }
                }

                atualizaSecundario();
                txtValoresPrimarios.Text = expressao;
                equal.Focus();
            };
            //potencia
            potencia.Click += (sender, e) =>
            {
                string[] partes = expressao.Split(new char[] { '+', '-', '*', '/' });
                double firstNumber;

                if (!double.TryParse(partes.First(), NumberStyles.Float, CultureInfo.GetCultureInfo("pt-BR"), out firstNumber))
                {
                    if (!double.TryParse(partes.First(), NumberStyles.Float, CultureInfo.GetCultureInfo("en-US"), out firstNumber))
                    {
                        return;
                    }
                }

                string lastNumber = partes.LastOrDefault();
                int ultimoOperadorIndex = expressao.LastIndexOfAny(new char[] { '+', '-', '*', '/' });

                if (partes.Length > 1 && !string.IsNullOrEmpty(lastNumber))
                {
                    double lastNumberFormated;

                    if (!double.TryParse(lastNumber, NumberStyles.Float, CultureInfo.GetCultureInfo("pt-BR"), out lastNumberFormated))
                    {
                        if (!double.TryParse(lastNumber, NumberStyles.Float, CultureInfo.GetCultureInfo("en-US"), out lastNumberFormated))
                        {
                            return;
                        }
                    }

                    string operador = ultimoOperadorIndex >= 0 ? expressao.Substring(ultimoOperadorIndex, 1) : "";
                    double resultado = lastNumberFormated * lastNumberFormated;

                    expressao = $"{firstNumber}{operador}{resultado.ToString("G", CultureInfo.GetCultureInfo("pt-BR"))}";
                    atualizaPrimario();
                }
                else if (ultimoOperadorIndex >= 0)
                {
                    string operador = expressao[ultimoOperadorIndex].ToString();

                    switch (operador)
                    {
                        case "+":
                            expressao = (firstNumber + (firstNumber * firstNumber)).ToString("G", CultureInfo.GetCultureInfo("pt-BR"));
                            break;
                        case "-":
                            expressao = (firstNumber - (firstNumber * firstNumber)).ToString("G", CultureInfo.GetCultureInfo("pt-BR"));
                            break;
                        case "*":
                            expressao = (firstNumber * (firstNumber * firstNumber)).ToString("G", CultureInfo.GetCultureInfo("pt-BR"));
                            break;
                        case "/":
                            expressao = (firstNumber / (firstNumber * firstNumber)).ToString("G", CultureInfo.GetCultureInfo("pt-BR"));
                            break;
                    }
                }
                else
                {
                    expressao = (firstNumber * firstNumber).ToString("G", CultureInfo.GetCultureInfo("pt-BR"));
                }

                txtValoresSecundarios.Text = expressao;
                atualizaPrimario();
                equal.Focus();
                calculaResultado();
            };



            //Esc
            clear.Click += (sender, e) =>
            {
                expressao = "";
                txtValoresPrimarios.Text = expressao;
                txtValoresSecundarios.Text = "";
                equal.Focus();
            };
            //Del
            delete.Click += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(expressao))
                {
                    expressao = expressao.Substring(0, expressao.Length - 1);
                    atualizaSecundario();
                    atualizaPrimario();
                }

                equal.Focus();
            };
            //cancel
            cancel.Click += (sender, e) =>
            {
                if (expressao.Length > 0)
                {
                    int lastIndex = expressao.LastIndexOfAny(new char[] { '+', '-', '*', '/', '%', '√' });

                    if (lastIndex >= 0)
                    {
                        expressao = expressao.Substring(0, lastIndex + 1);
                        atualizaSecundario();
                        atualizaPrimario();
                    }
                    else
                    {
                        limpaExpressao();
                    }
                }
                equal.Focus();
            };

            this.KeyPreview = true;

            this.KeyDown += (sender, e) => TrataAtalhos(e);

        }

        private void atualizaPrimario()
        {
            string[] partes = expressao.Split(new char[] { '+', '-', '*', '/' });
            string ultimoNumero = partes.Last();
            txtValoresPrimarios.Text = ultimoNumero;
        }
        private void atualizaSecundario()
        {
            txtValoresSecundarios.Text = expressao;
        }

        private void limpaExpressao()
        {
            expressao = "";
            atualizaSecundario();
        }

        private void defineValores(string valor)
        {
            //Limpa a expressão após o resultado se digitar algum número
            if (result)
            {
                limpaExpressao();
                result = false;
            }
            expressao += valor;
            atualizaSecundario();
            atualizaPrimario();
        }

        private void defineOperador(string operador)
        {
            //Força operação sem limpar a expressão
            if (expressao.Any(c => "+-*/".Contains(c)))
            {
                calculaResultado();
                txtValoresSecundarios.Text = expressao + operador;
            }
            //Mantém uma nova operação caso pressionado um novo operador
            if (result)
            {
                result = false;
                expressao += operador;
            }
            //Troca o operador caso pressionar um novo
            else if (expressao.Length > 0 && "+-*/%".Contains(expressao[expressao.Length - 1]))
            {
                expressao = expressao.Substring(0, expressao.Length - 1) + operador;
            }
            //Adiciona 0, caso o último caractere for virgula
            else if (expressao.Length > 0 && (expressao[expressao.Length - 1] == ','))
            {
                expressao += "0" + operador;
            }
            //Adiciona 0 se pressionar um operador e não tem outro caractere
            else if (expressao.Length == 0)
            {
                expressao += "0" + operador;
            }
            //Adiciona o operador
            else
            {
                expressao += operador;
            }

            atualizaSecundario();
            txtValoresPrimarios.Text = "";

        }

        private void definePerc(string perc)
        {
            if (result)
            {
                result = false;
                expressao += perc;
            }

            else if (expressao.Length > 0)
            {
                string[] partes = expressao.Split(new char[] { '+', '-', '*', '/' });
                if ("+-*/%".Contains(expressao[expressao.Length - 1]))
                {   //Faz os cálculos quando tem o valor + algum operador
                    double firstNumber = double.Parse(partes.First(), CultureInfo.GetCultureInfo("pt-BR"));
                    if (expressao.Contains("-"))
                    {
                        expressao = expressao.Substring(0, expressao.Length - 1);
                        expressao = (firstNumber - (firstNumber * firstNumber) / 100).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                    if (expressao.Contains("+"))
                    {
                        expressao = expressao.Substring(0, expressao.Length - 1);
                        expressao = (firstNumber + (firstNumber * firstNumber) / 100).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                    if (expressao.Contains("*"))
                    {
                        expressao = expressao.Substring(0, expressao.Length - 1);
                        expressao = (firstNumber * (firstNumber * 0.01)).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                    if (expressao.Contains("/"))
                    {
                        expressao = expressao.Substring(0, expressao.Length - 1);
                        expressao = (firstNumber / (firstNumber / 100)).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                }
                //Divide por 100 quando tem só um valor
                else if (expressao.All(c => char.IsDigit(c)))
                {
                    double firstNumber = double.Parse(partes.First(), CultureInfo.GetCultureInfo("pt-BR"));
                    expressao = (firstNumber / 100).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                }
                else
                {
                    expressao += perc;
                }
            }

            atualizaSecundario();
            atualizaPrimario();
            calculaResultado();
        }

        private void defineRaizQuadrada(string raiz)
        {
            int ultimoOperadorIndex = expressao.LastIndexOfAny(new char[] { '+', '-', '*', '/' });
            string[] partes = expressao.Split(new char[] { '+', '-', '*', '/' });
            string lastNumber = partes.LastOrDefault();
            string firstNumber = partes.First();
            //Mantém a operação
            if (result)
            {
                result = false;
                expressao = raiz + expressao;
            }
            else if (expressao.Length > 0 && ultimoOperadorIndex < 0)
            {
                // Se não houver operador e só tiver um número, calcula a raiz quadrada
                double numero = double.Parse(firstNumber, CultureInfo.GetCultureInfo("pt-BR"));
                double resultadoRaiz = Math.Sqrt(numero);
                expressao = resultadoRaiz.ToString(CultureInfo.GetCultureInfo("pt-BR"));
            }
            else
            {
                //Se expressao for maior que 0 e ter caractere após o operador, cai aqui
                if (ultimoOperadorIndex >= 0 && !string.IsNullOrEmpty(lastNumber))
                {
                    double numero = double.Parse(lastNumber, CultureInfo.GetCultureInfo("pt-BR"));
                    double resultadoRaiz = Math.Sqrt(numero);
                    expressao = expressao.Substring(0, ultimoOperadorIndex + 1) + resultadoRaiz.ToString(CultureInfo.GetCultureInfo("pt-BR"));
                }
                //Calcula raiz do valor informado, quando tem valor + operador
                else if (expressao.Length > 0 && !expressao.Contains("+-*/"))
                {
                    double numero = double.Parse(firstNumber, CultureInfo.GetCultureInfo("pt-BR"));
                    double resultadoRaiz = Math.Sqrt(numero);

                    if (expressao.Contains("+"))
                    {
                        expressao = expressao.Substring(0, expressao.Length - firstNumber.Length) + "+" + resultadoRaiz.ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                    if (expressao.Contains("-"))
                    {
                        expressao = expressao.Substring(0, expressao.Length - firstNumber.Length) + "-" + resultadoRaiz.ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                    if (expressao.Contains("*"))
                    {
                        expressao = expressao.Substring(0, expressao.Length - firstNumber.Length) + "*" + resultadoRaiz.ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                    if (expressao.Contains("/"))
                    {
                        expressao = expressao.Substring(0, expressao.Length - firstNumber.Length) + "/" + resultadoRaiz.ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                }
            }
            atualizaSecundario();
            atualizaPrimario();
            calculaResultado();
        }

        private void trataVirgula(string virgula)
        {
            //Limpa a operação
            if (result)
            {
                limpaExpressao();
                result = false;
            }

            if (expressao.Length > 0)
            {
                //Se último caractere for um operador, coloca 0 + ,
                if ("+-*/%".Contains(expressao[expressao.Length - 1]))
                {
                    expressao += "0" + virgula;
                }
                //Permite colocar uma vírgula por número
                else
                {
                    string[] partes = expressao.Split(new char[] { '+', '-', '*', '/' });
                    string ultimoNumero = partes.Last();

                    if (!ultimoNumero.Contains(","))
                    {
                        expressao += virgula;
                    }
                }
            }
            //Quando não tem a expressão, coloca 0 + ,
            else
            {
                expressao += "0" + virgula;
            }

            atualizaSecundario();
            atualizaPrimario();
        }

        private void calculaResultado()
        {
            try
            {
                txtValoresSecundarios.Text = expressao;
                //Trata quando tem valor + operador
                if (expressao.Length > 0 && "+-*/".Contains(expressao[expressao.Length - 1]))
                {
                    expressao = expressao.Substring(0, expressao.Length - 1);
                }
                if (expressao.Contains("%"))
                {
                    string[] partes = expressao.Split(new char[] { '+', '-', '*', '/' });

                    if (!expressao.Contains("+-*/"))
                    {
                        double lastNumber = double.Parse(partes.Last().Trim('%'), CultureInfo.GetCultureInfo("pt-BR"));
                        double firstNumber = double.Parse(partes.First(), CultureInfo.GetCultureInfo("pt-BR"));

                        double valorPerc = firstNumber * lastNumber / 100;
                        //Faz os cálculos de % dependendo do operador
                        if (expressao.Contains("-"))
                        {
                            expressao = (firstNumber - valorPerc).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                        }
                        else if (expressao.Contains("*"))
                        {
                            expressao = (firstNumber * (lastNumber / 100)).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                        }
                        else if (expressao.Contains("/"))
                        {
                            expressao = (firstNumber / (lastNumber / 100)).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                        }
                        else
                        {
                            expressao = (firstNumber + valorPerc).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                        }
                    }
                    else
                    {
                        double firstNumber = double.Parse(partes.First(), CultureInfo.GetCultureInfo("pt-BR"));
                        expressao = (firstNumber / 100).ToString(CultureInfo.GetCultureInfo("pt-BR"));
                    }
                }
                else if (expressao.Contains("√"))
                {
                    string[] partes = expressao.Split(new char[] { '+', '-', '*', '/' });
                    string lastNumber = partes.LastOrDefault();
                    string firstNumber = partes.First();
                    //Calcula a raiz do último número informado após um operador
                    if (!string.IsNullOrEmpty(lastNumber))
                    {
                        expressao = calcularRaizQuadrada(lastNumber);
                    }
                    else
                    {
                        expressao = calcularRaizQuadrada(firstNumber);
                    }
                }

                expressao = expressao.Replace(",", ".");
                var resultado = EvaluateExpression(expressao);
                //Formata o resultado com casa de milhar e até 5 decimais
                string resultadoFormatado = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:#,##0.#####}", resultado);
                txtValoresPrimarios.Text = resultadoFormatado;
                expressao = resultado.ToString();
                result = true;
            }
            catch (Exception ex)
            {
                txtValoresPrimarios.Text = "";
                txtValoresSecundarios.Text = "";
                expressao = "";
            }
        }
        private string calcularRaizQuadrada(string expressao)
        {
            int indiceInicio = expressao.IndexOf("√") + 1;

            if (indiceInicio > 0 && indiceInicio < expressao.Length)
            {
                string numeroRaizStr = expressao.Substring(indiceInicio);

                if (double.TryParse(numeroRaizStr, NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("pt-BR"), out double numeroRaiz))
                {
                    double resultadoRaiz = Math.Sqrt(numeroRaiz);
                    return resultadoRaiz.ToString(CultureInfo.GetCultureInfo("pt-BR"));
                }
            }

            return expressao;
        }

        //Faz os cálculos
        private double EvaluateExpression(string expressao)
        {
            DataTable table = new DataTable();
            expressao = expressao.Replace("√", "SQRT");
            return Convert.ToDouble(table.Compute(expressao, ""));
        }

        //Atalhos
        private void TrataAtalhos(KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    zero.PerformClick();
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    one.PerformClick();
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    two.PerformClick();
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    three.PerformClick();
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    four.PerformClick();
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    if (e.Shift)
                    {
                        perc.PerformClick();
                    }
                    else
                    {
                        five.PerformClick();
                    }
                    break;
                case Keys.Oem5:
                    if (e.Shift)
                    {
                        perc.PerformClick();
                    }
                    break;
                case Keys.R:
                    if (e.Shift)
                    {
                        raizQuadrada.PerformClick();
                    }
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    six.PerformClick();
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    seven.PerformClick();
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    eigth.PerformClick();
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    nine.PerformClick();
                    break;
                case Keys.Oemcomma:
                case Keys.Decimal:
                    comma.PerformClick();
                    break;
                case Keys.Add:
                    mais.PerformClick();
                    break;
                case Keys.Subtract:
                    menos.PerformClick();
                    break;
                case Keys.Multiply:
                    vezes.PerformClick();
                    break;
                case Keys.Divide:
                    divisao.PerformClick();
                    break;
                case Keys.Escape:
                case Keys.Delete:
                    clear.PerformClick();
                    break;
                case Keys.Oemplus:
                case Keys.Enter:
                    equal.PerformClick();
                    break;
                case Keys.Back:
                    delete.PerformClick();
                    break;
                case Keys.E:
                    if (e.Shift)
                    {
                        maisMenos.PerformClick();
                    }
                    break;
                case Keys.P:
                    if (e.Shift)
                    {
                        potencia.PerformClick();
                    }
                    break;
            }
        }
        //Deixa o Form em degrade
        private void degradeForm(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Rectangle gradient_rect = new Rectangle(0, 0, Width, Height);

            Brush br = new LinearGradientBrush(gradient_rect, Color.DimGray, Color.DarkGray, 90f);

            graphics.FillRectangle(br, gradient_rect);
        }
    }
}