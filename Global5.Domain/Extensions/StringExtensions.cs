using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Global5.Domain.Extensions
{
    public class StringExtensions
    {
        public static string RemoveAccent(string str)
        {
            string[] acentos = new string[] { "ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û" };
            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };

            for (int i = 0; i < acentos.Length; i++)
                str = str.Replace(acentos[i], semAcento[i]);

            return str;
        }
        public static bool IsNumeric(string input)
        {
            bool validate = false;

            if (!string.IsNullOrEmpty(input))
            {
                long valor = 0;

                if (long.TryParse(input, out valor))
                {
                    validate = true;
                }
            }
            return validate;
        }
        public static bool ValidateLenght(string input, int lenght)
        {
            bool lenghtValidate = false;

            if (!string.IsNullOrEmpty(input))
            {
                lenghtValidate = true;

                if (input.Length != lenght)
                {
                    lenghtValidate = false;
                }
            }
            return lenghtValidate;
        }
        public static string AddZeroToLeft(string input, int lenghtText)
        {
            string inputData = input;

            if (!string.IsNullOrEmpty(input))
            {
                inputData = inputData.PadLeft(lenghtText, '0');
            }
            return inputData;
        }
        public static string AddZeroToRight(string input, int lenghtText)
        {
            string inputData = input;

            if (!string.IsNullOrEmpty(input))
            {
                inputData = inputData.PadRight(lenghtText, '0');
            }
            return inputData;
        }

        public static string ConvertValueStringToScreen(string input)
        {
            string inputString = "0";

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    inputString = input.Replace(".", ",");
                }
            }
            catch { }

            return inputString;
        }
        public static string ConvertValueStringToSql(string input)
        {
            string inputString = "0";

            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    inputString = input.Replace(".", "");
                    inputString = inputString.Replace(",", ".");
                }
            }
            catch { }

            return inputString;
        }

        public static string ConvertDecimalToSql(decimal input)
        {
            string inputString = "0";

            if (input > 0)
            {
                try
                {
                    inputString = input.ToString().Replace(",", ".");
                }
                catch { }
            }
            return inputString;
        }
        public static DateTimeOffset? ConvertIntToDateTime(int input)
        {
            DateTimeOffset? inputString = null;

            if (input > 0)
            {
                try
                {
                    inputString = DateTime.ParseExact(20131108.ToString(), "yyyyMMdd", null);
                }
                catch { }
            }
            return inputString;
        }
        public static string FormatDateToBrazil(DateTimeOffset dateTime)
        {
            string dateFormat = "";

            if (IsDate(dateTime.ToString("dd/MM/yyyy")))
            {
                if (dateTime.Year != 1)
                {
                    dateFormat = dateTime.ToString("dd/MM/yyyy");
                }
            }
            return dateFormat;
        }
        public static string FormatDateTimeToBrazil(DateTimeOffset dateTime)
        {
            string dateFormat = "";

            if (IsDate(dateTime.ToString("dd/MM/yyyy")))
            {
                if (dateTime.Year != 1)
                {
                    dateFormat = dateTime.ToString("dd/MM/yyyy HH:mm");
                }
            }
            return dateFormat;
        }
        public static bool IsDate(string input)
        {
            bool noIsDate = false;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var day = input.ToString().Substring(0, 2);
                var month = input.ToString().Substring(3, 2);
                var year = input.ToString().Substring(6, 4);

                if (!string.IsNullOrWhiteSpace(day) && !string.IsNullOrWhiteSpace(month) && !string.IsNullOrWhiteSpace(year))
                {
                    if (Convert.ToInt16(day) > 0 && Convert.ToInt16(day) <= 31)
                    {
                        if (Convert.ToInt16(month) > 0 && Convert.ToInt16(month) <= 12)
                        {
                            if (Convert.ToInt16(year) > 0 && Convert.ToInt16(year) <= Convert.ToInt32(DateTime.Now.ToString("yyyy")))
                            {
                                noIsDate = true;
                            }
                        }
                    }
                }
            }
            return noIsDate;
        }
        public static string MaskEmail(string email)
        {
            string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
            string result = Regex.Replace(email, pattern, m => new string('*', m.Length));

            return result;
        }
        public static string FormatTimeToBrazil(DateTimeOffset dateTime)
        {
            string timeFormat = "";

            if (IsDate(dateTime.ToString("dd/MM/yyyy")))
            {
                if (dateTime.Year != 1)
                {
                    timeFormat = dateTime.ToString("HH:mm");
                }
            }
            return timeFormat;
        }
        public static string DayOfWeek(DateTimeOffset input)
        {
            string dayOfWeek = "";

            if (IsDate(input.ToString("dd/MM/yyyy")))
            {
                if (input.Year != 1)
                {
                    DayOfWeek wk = input.DayOfWeek;

                    switch (wk)
                    {
                        case System.DayOfWeek.Sunday:

                            dayOfWeek = "Domingo";
                            break;

                        case System.DayOfWeek.Monday:

                            dayOfWeek = "Segunda-feira";
                            break;

                        case System.DayOfWeek.Tuesday:

                            dayOfWeek = "Terça-feira";
                            break;

                        case System.DayOfWeek.Wednesday:

                            dayOfWeek = "Quarta-feira";
                            break;

                        case System.DayOfWeek.Thursday:

                            dayOfWeek = "Quinta-feira";
                            break;

                        case System.DayOfWeek.Friday:

                            dayOfWeek = "Sexta-feira";
                            break;

                        case System.DayOfWeek.Saturday:

                            dayOfWeek = "Sábado";
                            break;

                        default:
                            break;
                    }
                }
            }
            return dayOfWeek;
        }
        public static string OneInitialName(string input)
        {
            var initialName = "";

            if (!string.IsNullOrWhiteSpace(input))
            {
                if (input.Substring(0, 1) == "(")
                {
                    initialName = input.Substring(1, 1);
                }
                else
                {
                    initialName = input.Substring(0, 1);
                }
            }
            return initialName;
        }
        public static string TwoInitialName(string input)
        {
            var initialName = "";

            if (!string.IsNullOrWhiteSpace(input))
            {
                if (input.Substring(0, 1) == "(")
                {
                    initialName = input.Substring(1, 2);
                }
                else
                {
                    initialName = input.Substring(0, 2);
                }
            }
            return initialName;
        }
        public static bool ValidateClaveFiscal(string clavefiscal)
        {
            bool rv = false;

            if (!string.IsNullOrWhiteSpace(clavefiscal))
            {
                int verificador;
                int resultado = 0;
                string cuit_nro = clavefiscal.Replace("-", string.Empty);
                string codes = "6789456789";
                long cuit_long = 0;
                if (long.TryParse(cuit_nro, out cuit_long))
                {
                    verificador = int.Parse(cuit_nro[cuit_nro.Length - 1].ToString());
                    int x = 0;
                    while (x < 10)
                    {

                        int digitoValidador = int.Parse(codes.Substring((x), 1));
                        int digito = int.Parse(cuit_nro.Substring((x), 1));
                        int digitoValidacion = digitoValidador * digito;
                        resultado += digitoValidacion;
                        x++;
                    }
                    resultado = resultado % 11;
                    rv = (resultado == verificador);
                }
            }
            return rv;
        }
        private static string CalculateDNILeter(int dniNumbers)
        {
            //Cargamos los digitos de control
            string[] control = { "T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B", "N", "J", "Z", "S", "Q", "V", "H", "L", "C", "K", "E" };
            var mod = dniNumbers % 23;
            return control[mod];
        }
        public static bool ValidateDNI(string dni = "", bool validateLetter = false)
        {
            if (!string.IsNullOrWhiteSpace(dni))
            {
                if (validateLetter)
                {
                    //Comprobamos si el DNI tiene 9 digitos
                    if (dni.Length != 9)
                    {
                        //No es un DNI Valido
                        return false;
                    }

                    //Extraemos los números y la letra
                    string dniNumbers = dni.Substring(0, dni.Length - 1);
                    string dniLeter = dni.Substring(dni.Length - 1, 1);
                    //Intentamos convertir los números del DNI a integer
                    var numbersValid = int.TryParse(dniNumbers, out int dniInteger);
                    if (!numbersValid)
                    {
                        //No se pudo convertir los números a formato númerico
                        return false;
                    }
                    if (CalculateDNILeter(dniInteger) != dniLeter)
                    {
                        //La letra del DNI es incorrecta
                        return false;
                    }
                }
                else
                {
                    //Comprobamos si el DNI tiene 9 digitos
                    if (dni.Length != 8)
                    {
                        //No es un DNI Valido
                        return false;
                    }
                }
            }
            //DNI Valido 🙂
            return true;
        }
        public static string ConvertYearMonthDayToDate(string input)
        {
            string dateformat = "";

            if (input != "")
            {
                var year = input.ToString().Substring(0, 4);
                var month = input.ToString().Substring(5, 2);
                var day = input.ToString().Substring(8, 2);

                dateformat = year + "/" + month + "/" + day;
            }
            return dateformat;
        }
        public static string ConvertYearMonthDayIntToDate(string input)
        {
            string dateformat = "";

            if (input != "")
            {
                var year = input.ToString().Substring(0, 4);
                var month = input.ToString().Substring(5, 2);
                var day = input.ToString().Substring(6, 2);

                dateformat = year + "/" + month + "/" + day;
            }
            return dateformat;
        }
        public static string ConvertYearMonthDayToDateBr(string input)
        {
            string dateformat = "0";

            if (!string.IsNullOrWhiteSpace(input))
            {
                var year = input.ToString().Substring(0, 4);
                var month = input.ToString().Substring(5, 2);
                var day = input.ToString().Substring(8, 2);

                if (!string.IsNullOrWhiteSpace(day) && !string.IsNullOrWhiteSpace(month) && !string.IsNullOrWhiteSpace(year))
                {
                    dateformat = day + "/" + month + "/" + year;
                }
            }
            return dateformat;
        }
        public static int ConvertYearMonthDayToDateInt(string input)
        {
            string dateformat = "0";

            if (!string.IsNullOrWhiteSpace(input))
            {
                var year = input.ToString().Substring(0, 4);
                var month = input.ToString().Substring(5, 2);
                var day = input.ToString().Substring(8, 2);

                if (!string.IsNullOrWhiteSpace(day) && !string.IsNullOrWhiteSpace(month) && !string.IsNullOrWhiteSpace(year))
                {
                    dateformat = year + month + day;
                }
            }
            return Convert.ToInt32(dateformat);
        }
        public static int CalculateAge(DateTime input)
        {
            int age = 0;

            try
            {
                var birthdate = new DateTime(input.Year, input.Month, input.Day);
                age = DateTime.Now.Year - birthdate.Year;
                if (birthdate > DateTime.Now.AddYears(-age))
                {
                    age--;
                }
            }
            catch (Exception)
            {
                age = -1;
            }
            return age;
        }
        public static string ValidateZipCode(string input)
        {
            string zipCode = "";

            if (!string.IsNullOrEmpty(input))
            {
                int valor = 0;

                if (int.TryParse(input, out valor))
                {
                    zipCode = input;
                }
            }
            return zipCode;
        }
        public static bool ValidateEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var ReGexEmail = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                return Regex.IsMatch(email, ReGexEmail);
            }
            else
            {
                return false;
            }
        }
        public static bool ValidateFullName(string input)
        {
            string pattern = string.Empty;

            input.Trim().Split(' ').ToList().ForEach(x =>
            {
                pattern += !string.IsNullOrEmpty(x) ? @"[A-Z][a-z].+ " : "";
            });

            pattern = pattern.Substring(0, pattern.Length - 1);
            Regex rgx = new Regex(pattern);

            return rgx.Match(input).Success;
        }
    }
}