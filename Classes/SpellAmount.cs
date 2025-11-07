namespace AdvanceAPI.Classes
{
    public static class SpellAmount
    {
        public static string InWrods(decimal amount)
        {
            string text = "";
            string text2 = "";
            string result = "";
            text = amount.ToString();
            int num = amount.ToString().IndexOf(".", 0);
            text2 = amount.ToString().Substring(num + 1);
            if (text == text2)
            {
                text2 = "";
            }
            else
            {
                text = amount.ToString().Substring(0, amount.ToString().IndexOf(".", 0));
                text = text.Replace(",", "").ToString();
            }

            switch (text.Length)
            {
                case 9:
                    result = F_Crores(text, text2);
                    break;
                case 8:
                    result = F_Crore(text, text2);
                    break;
                case 7:
                    result = F_Lakhs(text, text2);
                    break;
                case 6:
                    result = F_Lakh(text, text2);
                    break;
                case 5:
                    result = F_Thousands(text, text2);
                    break;
                case 4:
                    result = F_Thousand(text, text2);
                    break;
                case 3:
                    result = F_Hundred(text, text2);
                    break;
                case 2:
                    result = F_Number(text, text2);
                    break;
                case 1:
                    result = F_Number("0" + text, text2);
                    break;
            }

            return result;
        }

        public static string Tens(string s_amt)
        {
            string text = "";
            return s_amt switch
            {
                "0" => "",
                "1" => "One",
                "2" => "Two",
                "3" => "Three",
                "4" => "Four",
                "5" => "Five",
                "6" => "Six",
                "7" => "Seven",
                "8" => "Eight",
                "9" => "Nine",
                "10" => "Ten",
                "11" => "Eleven",
                "12" => "Twelve",
                "13" => "Thirteen",
                "14" => "Forteen",
                "15" => "Fifteen",
                "16" => "Sixteen",
                "17" => "Seventeen",
                "18" => "Eighteen",
                "19" => "Nineteen",
                "20" => "Twenty",
                "30" => "Thirty",
                "40" => "Forty",
                "50" => "Fifty",
                "60" => "Sixty",
                "70" => "Seventy",
                "80" => "Eighty",
                "90" => "Ninety",
                _ => "Nothing",
            };
        }

        public static string Word_Spell_Tens(string amt)
        {
            string text = null;
            string text2 = null;
            string text3 = null;
            int num = 0;
            num = Convert.ToInt32(amt.Substring(0, 2));
            if (num > 20)
            {
                text = amt.Substring(0, 1) + "0";
                text2 = amt.Substring(1, 1);
                return Tens(text) + " " + Tens(text2);
            }

            return Tens(num.ToString());
        }

        public static string F_Crores(string amt, string amt_paisa)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            string text5 = "";
            string text6 = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            if (amt_paisa == "")
            {
                text = Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Crores";
                if (amt.Substring(2, 7) != "0000000")
                {
                    if (amt.Substring(2, 2) != "00")
                    {
                        if (amt.Substring(2, 1) != "0")
                        {
                            num2 = Convert.ToInt32(amt.Substring(2, 2));
                            text2 = ((!(amt.Substring(4, 5) == "00000")) ? (" " + Word_Spell_Tens(num2.ToString()) + " Lakhs") : (" and " + Word_Spell_Tens(num2.ToString()) + " Lakhs"));
                        }
                        else
                        {
                            num2 = Convert.ToInt32(amt.Substring(3, 1));
                            text2 = ((!(amt.Substring(4, 5) == "00000")) ? (" " + Tens(num2.ToString())) : (" and " + Tens(num2.ToString())));
                            text2 = ((num2 <= 1) ? (text2 + " Lakh") : (text2 + " Lakhs"));
                        }
                    }

                    if (amt.Substring(4, 2) != "00")
                    {
                        if (amt.Substring(4, 1) != "0")
                        {
                            num3 = Convert.ToInt32(amt.Substring(4, 2));
                            text3 = ((!(amt.Substring(6, 3) == "000")) ? (" " + Word_Spell_Tens(num3.ToString()) + " Thousands") : (" and " + Word_Spell_Tens(num3.ToString()) + " Thousands"));
                        }
                        else
                        {
                            num3 = Convert.ToInt32(amt.Substring(5, 1));
                            text3 = ((!(amt.Substring(4, 5) == "000")) ? (" " + Tens(num3.ToString())) : (" and " + Tens(num3.ToString())));
                            text3 = ((num3 <= 1) ? (text3 + " Thousand") : (text3 + " Thousands"));
                        }
                    }

                    if (amt.Substring(6, 3) != "000")
                    {
                        if (amt.Substring(6, 1) != "0")
                        {
                            num4 = Convert.ToInt32(amt.Substring(6, 1));
                            text4 = ((num4 > 1) ? ((!(amt.Substring(7, 2) == "00")) ? (" " + Tens(num4.ToString()) + " Hundreds") : (" and" + Tens(num4.ToString()) + " Hundreds")) : ((!(amt.Substring(7, 2) == "00")) ? (" " + Tens(num4.ToString()) + " Hundred") : (" and" + Tens(num4.ToString()) + " Hundred")));
                        }

                        if (amt.Substring(7, 2) != "00")
                        {
                            num5 = Convert.ToInt32(amt.Substring(7, 2));
                            text5 = ((Convert.ToInt32(amt.Substring(7, 1)) == 0) ? (" and " + Tens(num5.ToString())) : (" and " + Word_Spell_Tens(num5.ToString())));
                        }
                    }
                }
            }
            else if (amt_paisa != "")
            {
                text = Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Crores";
                if (amt.Substring(2, 7) != "0000000")
                {
                    if (amt.Substring(2, 2) != "00")
                    {
                        if (amt.Substring(2, 1) != "0")
                        {
                            text2 = " " + Word_Spell_Tens(Convert.ToInt32(amt.Substring(2, 2)).ToString()) + " Lakhs";
                        }
                        else
                        {
                            num2 = Convert.ToInt32(amt.Substring(3, 1));
                            text2 = ((num2 <= 1) ? (" " + Tens(num2.ToString()) + " Lakh") : (" " + Tens(num2.ToString()) + " Lakhs"));
                        }
                    }

                    if (amt.Substring(4, 2) != "00")
                    {
                        if (amt.Substring(4, 1) != "0")
                        {
                            text3 = " " + Word_Spell_Tens(Convert.ToInt32(amt.Substring(4, 2)).ToString()) + " Thousands";
                        }
                        else
                        {
                            num3 = Convert.ToInt32(amt.Substring(5, 1));
                            text3 = ((num3 <= 1) ? (" " + Tens(num3.ToString()) + " Thousand") : (" " + Tens(num3.ToString()) + " Thousands"));
                        }
                    }

                    if (amt.Substring(6, 3) != "000")
                    {
                        if (amt.Substring(6, 1) != "0")
                        {
                            num4 = Convert.ToInt32(amt.Substring(6, 1));
                            text4 = ((num4 <= 1) ? (" " + Tens(num4.ToString()) + " Hundred") : (" " + Tens(num4.ToString()) + " Hundreds"));
                        }

                        if (amt.Substring(7, 2) != "00")
                        {
                            num5 = Convert.ToInt32(amt.Substring(7, 2));
                            text5 = ((!(amt.Substring(7, 1) != "0")) ? (" " + Tens(num5.ToString())) : (" " + Word_Spell_Tens(num5.ToString())));
                        }
                    }
                }

                if (amt_paisa.Substring(0, 2) != "00")
                {
                    text6 = ((!(amt_paisa.Substring(0, 1) != "0")) ? (" " + Tens(amt_paisa.Substring(0, 2)) + " Paisa") : (" and " + Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa"));
                }
            }

            return "Taka " + text + text2 + text3 + text4 + text5 + text6 + " Only";
        }

        public static string F_Crore(string amt, string amt_paisa)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            string text5 = "";
            string text6 = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            if (amt_paisa == "")
            {
                num = Convert.ToInt32(amt.Substring(0, 1));
                text = ((num <= 1) ? (Tens(num.ToString()) + " Crore") : (Tens(num.ToString()) + " Crores"));
                if (amt.Substring(1, 7) != "0000000")
                {
                    if (amt.Substring(1, 2) != "00")
                    {
                        if (amt.Substring(1, 1) != "0")
                        {
                            num2 = Convert.ToInt32(amt.Substring(1, 2));
                            text2 = ((!(amt.Substring(3, 5) == "00000")) ? (" " + Word_Spell_Tens(num2.ToString()) + " Lakhs") : (" and " + Word_Spell_Tens(num2.ToString()) + " Lakhs"));
                        }
                        else
                        {
                            num2 = Convert.ToInt32(amt.Substring(2, 1));
                            text2 = ((!(amt.Substring(3, 5) == "00000")) ? (" " + Tens(num2.ToString())) : (" and " + Tens(num2.ToString())));
                            text2 = ((num2 <= 1) ? (text2 + " Lakh") : (text2 + " Lakhs"));
                        }
                    }

                    if (amt.Substring(3, 2) != "00")
                    {
                        if (amt.Substring(3, 1) != "0")
                        {
                            num3 = Convert.ToInt32(amt.Substring(3, 2));
                            text3 = ((!(amt.Substring(5, 3) == "000")) ? (" " + Word_Spell_Tens(num3.ToString()) + " Thousands") : (" and " + Word_Spell_Tens(num3.ToString()) + " Thousands"));
                        }
                        else
                        {
                            num3 = Convert.ToInt32(amt.Substring(4, 1));
                            text3 = ((!(amt.Substring(5, 3) == "000")) ? (" " + Tens(num3.ToString())) : (" and " + Tens(num3.ToString())));
                            text3 = ((num3 <= 1) ? (text3 + " Thousand") : (text3 + " Thousands"));
                        }
                    }

                    if (amt.Substring(5, 3) != "000")
                    {
                        if (amt.Substring(5, 1) != "0")
                        {
                            num4 = Convert.ToInt32(amt.Substring(5, 1));
                            text4 = ((num4 > 1) ? ((!(amt.Substring(6, 2) == "00")) ? (" " + Tens(num4.ToString()) + " Hundreds") : (" and" + Tens(num4.ToString()) + " Hundreds")) : ((!(amt.Substring(6, 2) == "00")) ? (" " + Tens(num4.ToString()) + " Hundred") : (" and" + Tens(num4.ToString()) + " Hundred")));
                        }

                        if (amt.Substring(6, 2) != "00")
                        {
                            num5 = Convert.ToInt32(amt.Substring(6, 2));
                            text5 = ((Convert.ToInt32(amt.Substring(6, 1)) == 0) ? (" and " + Tens(num5.ToString())) : (" and " + Word_Spell_Tens(num5.ToString())));
                        }
                    }
                }
            }
            else if (amt_paisa != "")
            {
                num = Convert.ToInt32(amt.Substring(0, 1));
                text = ((num <= 1) ? (Tens(num.ToString()) + " Crore") : (Tens(num.ToString()) + " Crores"));
                if (amt.Substring(1, 7) != "0000000")
                {
                    if (amt.Substring(1, 2) != "00")
                    {
                        if (amt.Substring(1, 1) != "0")
                        {
                            text2 = " " + Word_Spell_Tens(Convert.ToInt32(amt.Substring(1, 2)).ToString()) + " Lakhs";
                        }
                        else
                        {
                            num2 = Convert.ToInt32(amt.Substring(2, 1));
                            text2 = ((num2 <= 1) ? (" " + Tens(num2.ToString()) + " Lakh") : (" " + Tens(num2.ToString()) + " Lakhs"));
                        }
                    }

                    if (amt.Substring(3, 2) != "00")
                    {
                        if (amt.Substring(3, 1) != "0")
                        {
                            text3 = " " + Word_Spell_Tens(Convert.ToInt32(amt.Substring(3, 2)).ToString()) + " Thousands";
                        }
                        else
                        {
                            num3 = Convert.ToInt32(amt.Substring(4, 1));
                            text3 = ((num3 <= 1) ? (" " + Tens(num3.ToString()) + " Thousand") : (" " + Tens(num3.ToString()) + " Thousands"));
                        }
                    }

                    if (amt.Substring(5, 3) != "000")
                    {
                        if (amt.Substring(5, 1) != "0")
                        {
                            num4 = Convert.ToInt32(amt.Substring(5, 1));
                            text4 = ((num4 <= 1) ? (" " + Tens(num4.ToString()) + " Hundred") : (" " + Tens(num4.ToString()) + " Hundreds"));
                        }

                        if (amt.Substring(6, 2) != "00")
                        {
                            num5 = Convert.ToInt32(amt.Substring(6, 2));
                            text5 = ((!(amt.Substring(6, 1) != "0")) ? (" " + Tens(num5.ToString())) : (" " + Word_Spell_Tens(num5.ToString())));
                        }
                    }
                }

                if (amt_paisa.Substring(0, 2) != "00")
                {
                    text6 = ((!(amt_paisa.Substring(0, 1) != "0")) ? (" " + Tens(amt_paisa.Substring(0, 2)) + " Paisa") : (" and " + Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa"));
                }
            }

            return "Taka " + text + text2 + text3 + text4 + text5 + text6 + " Only";
        }

        public static string F_Lakhs(string amt, string amt_paisa)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            string text5 = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 2) != "00" && amt.Substring(0, 1) != "0")
                {
                    text = Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Lakhs";
                }

                if (amt.Substring(2, 2) != "00")
                {
                    if (amt.Substring(2, 1) != "0")
                    {
                        num2 = Convert.ToInt32(amt.Substring(2, 2));
                        text2 = ((!(amt.Substring(4, 3) == "000")) ? (" " + Word_Spell_Tens(num2.ToString()) + " Thousands") : (" and " + Word_Spell_Tens(num2.ToString()) + " Thousands"));
                    }
                    else
                    {
                        num2 = Convert.ToInt32(amt.Substring(3, 1));
                        text2 = ((!(amt.Substring(4, 3) == "000")) ? (" " + Tens(num2.ToString())) : (" and " + Tens(num2.ToString())));
                        text2 = ((num2 <= 1) ? (text2 + " Thousand") : (text2 + " Thousands"));
                    }
                }

                if (amt.Substring(4, 3) != "000")
                {
                    if (amt.Substring(4, 1) != "0")
                    {
                        num3 = Convert.ToInt32(amt.Substring(4, 1));
                        text3 = ((num3 > 1) ? ((!(amt.Substring(5, 2) == "00")) ? (" " + Tens(num3.ToString()) + " Hundreds") : (" and" + Tens(num3.ToString()) + " Hundreds")) : ((!(amt.Substring(5, 2) == "00")) ? (" " + Tens(num3.ToString()) + " Hundred") : (" and" + Tens(num3.ToString()) + " Hundred")));
                    }

                    if (amt.Substring(5, 2) != "00")
                    {
                        num4 = Convert.ToInt32(amt.Substring(5, 2));
                        text4 = ((Convert.ToInt32(amt.Substring(5, 1)) == 0) ? (" and " + Tens(num4.ToString())) : (" and " + Word_Spell_Tens(num4.ToString())));
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 2) != "00" && amt.Substring(0, 1) != "0")
                {
                    text = " " + Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Lakhs";
                }

                if (amt.Substring(2, 2) != "00")
                {
                    if (amt.Substring(2, 1) != "0")
                    {
                        text2 = " " + Word_Spell_Tens(Convert.ToInt32(amt.Substring(2, 2)).ToString()) + " Thousands";
                    }
                    else
                    {
                        num2 = Convert.ToInt32(amt.Substring(3, 1));
                        text2 = ((num2 <= 1) ? (" " + Tens(num2.ToString()) + " Thousand") : (" " + Tens(num2.ToString()) + " Thousands"));
                    }
                }

                if (amt.Substring(4, 3) != "000")
                {
                    if (amt.Substring(4, 1) != "0")
                    {
                        num3 = Convert.ToInt32(amt.Substring(4, 1));
                        text3 = ((num3 <= 1) ? (" " + Tens(num3.ToString()) + " Hundred") : (" " + Tens(num3.ToString()) + " Hundreds"));
                    }

                    if (amt.Substring(5, 2) != "00")
                    {
                        num4 = Convert.ToInt32(amt.Substring(5, 2));
                        text4 = ((!(amt.Substring(5, 1) != "0")) ? (" " + Tens(num4.ToString())) : (" " + Word_Spell_Tens(num4.ToString())));
                    }
                }

                if (amt_paisa.Substring(0, 2) != "00")
                {
                    text5 = ((!(amt_paisa.Substring(0, 1) != "0")) ? (" " + Tens(amt_paisa.Substring(0, 2)) + " Paisa") : (" and " + Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa"));
                }
            }

            return "Taka " + text + text2 + text3 + text4 + text5 + " Only";
        }

        public static string F_Lakh(string amt, string amt_paisa)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            string text5 = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    num = Convert.ToInt32(amt.Substring(0, 1));
                    text = ((num <= 1) ? (Tens(num.ToString()) + " Lakh") : (Tens(num.ToString()) + " Lakhs"));
                }

                if (amt.Substring(1, 2) != "00")
                {
                    if (amt.Substring(1, 1) != "0")
                    {
                        num2 = Convert.ToInt32(amt.Substring(1, 2));
                        text2 = ((!(amt.Substring(3, 3) == "000")) ? (" " + Word_Spell_Tens(num2.ToString()) + " Thousands") : (" and " + Word_Spell_Tens(num2.ToString()) + " Thousands"));
                    }
                    else
                    {
                        num2 = Convert.ToInt32(amt.Substring(2, 1));
                        text2 = ((!(amt.Substring(3, 3) == "000")) ? (" " + Tens(num2.ToString())) : (" and " + Tens(num2.ToString())));
                        text2 = ((num2 <= 1) ? (text2 + " Thousand") : (text2 + " Thousands"));
                    }
                }

                if (amt.Substring(3, 3) != "000")
                {
                    if (amt.Substring(3, 1) != "0")
                    {
                        num3 = Convert.ToInt32(amt.Substring(3, 1));
                        text3 = ((num3 > 1) ? ((!(amt.Substring(4, 2) == "00")) ? (" " + Tens(num3.ToString()) + " Hundreds") : (" and" + Tens(num3.ToString()) + " Hundreds")) : ((!(amt.Substring(4, 2) == "00")) ? (" " + Tens(num3.ToString()) + " Hundred") : (" and" + Tens(num3.ToString()) + " Hundred")));
                    }

                    if (amt.Substring(4, 2) != "00")
                    {
                        num4 = Convert.ToInt32(amt.Substring(4, 2));
                        text4 = ((Convert.ToInt32(amt.Substring(4, 1)) == 0) ? (" and " + Tens(num4.ToString())) : (" and " + Word_Spell_Tens(num4.ToString())));
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    num = Convert.ToInt32(amt.Substring(0, 1));
                    text = ((num <= 1) ? (Tens(num.ToString()) + " Lakh") : (Tens(num.ToString()) + " Lakhs"));
                }

                if (amt.Substring(1, 2) != "00")
                {
                    if (amt.Substring(1, 1) != "0")
                    {
                        text2 = " " + Word_Spell_Tens(Convert.ToInt32(amt.Substring(1, 2)).ToString()) + " Thousands";
                    }
                    else
                    {
                        num2 = Convert.ToInt32(amt.Substring(2, 1));
                        text2 = ((num2 <= 1) ? (" " + Tens(num2.ToString()) + " Thousand") : (" " + Tens(num2.ToString()) + " Thousands"));
                    }
                }

                if (amt.Substring(3, 3) != "000")
                {
                    if (amt.Substring(3, 1) != "0")
                    {
                        num3 = Convert.ToInt32(amt.Substring(3, 1));
                        text3 = ((num3 <= 1) ? (" " + Tens(num3.ToString()) + " Hundred") : (" " + Tens(num3.ToString()) + " Hundreds"));
                    }

                    if (amt.Substring(4, 2) != "00")
                    {
                        num4 = Convert.ToInt32(amt.Substring(4, 2));
                        text4 = ((!(amt.Substring(4, 1) != "0")) ? (" " + Tens(num4.ToString())) : (" " + Word_Spell_Tens(num4.ToString())));
                    }
                }

                if (amt_paisa.Substring(0, 2) != "00")
                {
                    text5 = ((!(amt_paisa.Substring(0, 1) != "0")) ? (" " + Tens(amt_paisa.Substring(0, 2)) + " Paisa") : (" and " + Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa"));
                }
            }

            return "Taka " + text + text2 + text3 + text4 + text5 + " Only";
        }

        public static string F_Thousands(string amt, string amt_paisa)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    text = Word_Spell_Tens(Convert.ToInt32(amt.Substring(0, 2)).ToString()) + " Thousands";
                }

                if (amt.Substring(2, 3) != "000")
                {
                    if (amt.Substring(2, 1) != "0")
                    {
                        num2 = Convert.ToInt32(amt.Substring(2, 1));
                        text2 = ((num2 > 1) ? ((!(amt.Substring(3, 2) == "00")) ? (" " + Tens(num2.ToString()) + " Hundreds") : (" and" + Tens(num2.ToString()) + " Hundreds")) : ((!(amt.Substring(3, 2) == "00")) ? (" " + Tens(num2.ToString()) + " Hundred") : (" and" + Tens(num2.ToString()) + " Hundred")));
                    }

                    if (amt.Substring(3, 2) != "00")
                    {
                        num3 = Convert.ToInt32(amt.Substring(3, 2));
                        text3 = ((Convert.ToInt32(amt.Substring(3, 1)) == 0) ? (" and " + Tens(num3.ToString())) : (" and " + Word_Spell_Tens(num3.ToString())));
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    num = Convert.ToInt32(amt.Substring(0, 2));
                    text = ((num <= 1) ? (Word_Spell_Tens(num.ToString()) + " Thousand") : (Word_Spell_Tens(num.ToString()) + " Thousands"));
                }

                if (amt.Substring(2, 3) != "000")
                {
                    if (amt.Substring(2, 1) != "0")
                    {
                        num2 = Convert.ToInt32(amt.Substring(2, 1));
                        text2 = ((num2 <= 1) ? (" " + Tens(num2.ToString()) + " Hundred") : (" " + Tens(num2.ToString()) + " Hundreds"));
                    }

                    if (amt.Substring(3, 2) != "00")
                    {
                        num3 = Convert.ToInt32(amt.Substring(3, 2));
                        text3 = ((!(amt.Substring(3, 1) != "0")) ? (" " + Tens(num3.ToString())) : (" " + Word_Spell_Tens(num3.ToString())));
                    }
                }

                if (amt_paisa.Substring(0, 2) != "00")
                {
                    text4 = ((!(amt_paisa.Substring(0, 1) != "0")) ? (" " + Tens(amt_paisa.Substring(0, 2)) + " Paisa") : (" and " + Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa"));
                }
            }

            return "Taka " + text + text2 + text3 + text4 + " Only";
        }

        public static string F_Thousand(string amt, string amt_paisa)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            string text4 = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    num = Convert.ToInt32(amt.Substring(0, 1));
                    text = ((num <= 1) ? (Tens(num.ToString()) + " Thousand") : (Tens(num.ToString()) + " Thousands"));
                }

                if (amt.Substring(1, 3) != "000")
                {
                    if (amt.Substring(1, 1) != "0")
                    {
                        num2 = Convert.ToInt32(amt.Substring(1, 1));
                        text2 = ((num2 > 1) ? ((!(amt.Substring(2, 2) == "00")) ? (" " + Tens(num2.ToString()) + " Hundreds") : (" and" + Tens(num2.ToString()) + " Hundreds")) : ((!(amt.Substring(2, 2) == "00")) ? (" " + Tens(num2.ToString()) + " Hundred") : (" and" + Tens(num2.ToString()) + " Hundred")));
                    }

                    if (amt.Substring(2, 2) != "00")
                    {
                        num3 = Convert.ToInt32(amt.Substring(2, 2));
                        text3 = ((Convert.ToInt32(amt.Substring(2, 1)) == 0) ? (" and " + Tens(num3.ToString())) : (" and " + Word_Spell_Tens(num3.ToString())));
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 1) != "0")
                {
                    num = Convert.ToInt32(amt.Substring(0, 1));
                    text = ((num <= 1) ? (Tens(num.ToString()) + " Thousand") : (Tens(num.ToString()) + " Thousands"));
                }

                if (amt.Substring(1, 3) != "000")
                {
                    if (amt.Substring(1, 1) != "0")
                    {
                        num2 = Convert.ToInt32(amt.Substring(1, 1));
                        text2 = ((num2 <= 1) ? (" " + Tens(num2.ToString()) + " Hundred") : (" " + Tens(num2.ToString()) + " Hundreds"));
                    }

                    if (amt.Substring(2, 2) != "00")
                    {
                        num3 = Convert.ToInt32(amt.Substring(2, 2));
                        text3 = ((!(amt.Substring(2, 1) != "0")) ? (" " + Tens(num3.ToString())) : (" " + Word_Spell_Tens(num3.ToString())));
                    }
                }

                if (amt_paisa.Substring(0, 2) != "00")
                {
                    text4 = ((!(amt_paisa.Substring(0, 1) != "0")) ? (" " + Tens(amt_paisa.Substring(0, 2)) + " Paisa") : (" and " + Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa"));
                }
            }

            return "Taka " + text + text2 + text3 + text4 + " Only";
        }

        public static string F_Hundred(string amt, string amt_paisa)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            int num = 0;
            int num2 = 0;
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 3) != "000")
                {
                    if (amt.Substring(0, 1) != "0")
                    {
                        num = Convert.ToInt32(amt.Substring(0, 1));
                        text = ((num <= 1) ? (Tens(num.ToString()) + " Hundred") : (Tens(num.ToString()) + " Hundreds"));
                    }

                    if (amt.Substring(1, 2) != "00")
                    {
                        num2 = Convert.ToInt32(amt.Substring(1, 2));
                        text2 = ((Convert.ToInt32(amt.Substring(1, 1)) == 0) ? (" and " + Tens(num2.ToString())) : (" and " + Word_Spell_Tens(num2.ToString())));
                    }
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 3) != "000")
                {
                    if (amt.Substring(0, 1) != "0")
                    {
                        num = Convert.ToInt32(amt.Substring(0, 1));
                        text = ((num <= 1) ? (Tens(num.ToString()) + " Hundred") : (Tens(num.ToString()) + " Hundreds"));
                    }

                    if (amt.Substring(1, 2) != "00")
                    {
                        num2 = Convert.ToInt32(amt.Substring(1, 2));
                        text2 = ((!(amt.Substring(1, 1) != "0")) ? (" " + Tens(num2.ToString())) : (" " + Word_Spell_Tens(num2.ToString())));
                    }
                }

                if (amt_paisa.Substring(0, 2) != "00")
                {
                    text3 = ((!(amt_paisa.Substring(0, 1) != "0")) ? (" " + Tens(amt_paisa.Substring(0, 2)) + " Paisa") : (" and " + Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa"));
                }
            }

            return "Taka " + text + text2 + text3 + " Only";
        }

        public static string F_Number(string amt, string amt_paisa)
        {
            string text = "";
            string text2 = "";
            int num = 0;
            if (amt_paisa == "")
            {
                if (amt.Substring(0, 2) != "00")
                {
                    num = Convert.ToInt32(amt.Substring(0, 2));
                    text = ((Convert.ToInt32(amt.Substring(0, 1)) == 0) ? Tens(num.ToString()) : Word_Spell_Tens(num.ToString()));
                }
                else
                {
                    text = " Zero ";
                }
            }
            else if (amt_paisa != "")
            {
                if (amt.Substring(0, 2) != "00")
                {
                    num = Convert.ToInt32(amt.Substring(0, 2));
                    text = ((!(amt.Substring(0, 1) != "0")) ? Tens(num.ToString()) : Word_Spell_Tens(num.ToString()));
                }
                else
                {
                    text = " Zero ";
                }

                if (amt_paisa.Substring(0, 2) != "00")
                {
                    text2 = ((!(amt_paisa.Substring(0, 1) != "0")) ? (" " + Tens(amt_paisa.Substring(0, 2)) + " Paisa") : (" and " + Word_Spell_Tens(amt_paisa.Substring(0, 2)) + " Paisa"));
                }
            }

            return "Taka " + text + text2 + " Only";
        }

        public static string comma(decimal amount)
        {
            string text = "";
            string text2 = "";
            string text3 = "";
            text2 = amount.ToString();
            int num = amount.ToString().IndexOf(".", 0);
            text3 = amount.ToString().Substring(num + 1);
            if (text2 == text3)
            {
                text3 = "";
            }
            else
            {
                text2 = amount.ToString().Substring(0, amount.ToString().IndexOf(".", 0));
                text2 = text2.Replace(",", "").ToString();
            }

            switch (text2.Length)
            {
                case 9:
                    if (text3 == "")
                    {
                        return text2.Substring(0, 2) + "," + text2.Substring(2, 2) + "," + text2.Substring(4, 2) + "," + text2.Substring(6, 3);
                    }

                    return text2.Substring(0, 2) + "," + text2.Substring(2, 2) + "," + text2.Substring(4, 2) + "," + text2.Substring(6, 3) + "." + text3;
                case 8:
                    if (text3 == "")
                    {
                        return text2.Substring(0, 1) + "," + text2.Substring(1, 2) + "," + text2.Substring(3, 2) + "," + text2.Substring(5, 3);
                    }

                    return text2.Substring(0, 1) + "," + text2.Substring(1, 2) + "," + text2.Substring(3, 2) + "," + text2.Substring(5, 3) + "." + text3;
                case 7:
                    if (text3 == "")
                    {
                        return text2.Substring(0, 2) + "," + text2.Substring(2, 2) + "," + text2.Substring(4, 3);
                    }

                    return text2.Substring(0, 2) + "," + text2.Substring(2, 2) + "," + text2.Substring(4, 3) + "." + text3;
                case 6:
                    if (text3 == "")
                    {
                        return text2.Substring(0, 1) + "," + text2.Substring(1, 2) + "," + text2.Substring(3, 3);
                    }

                    return text2.Substring(0, 1) + "," + text2.Substring(1, 2) + "," + text2.Substring(3, 3) + "." + text3;
                case 5:
                    if (text3 == "")
                    {
                        return text2.Substring(0, 2) + "," + text2.Substring(2, 3);
                    }

                    return text2.Substring(0, 2) + "," + text2.Substring(2, 3) + "." + text3;
                case 4:
                    if (text3 == "")
                    {
                        return text2.Substring(0, 1) + "," + text2.Substring(1, 3);
                    }

                    return text2.Substring(0, 1) + "," + text2.Substring(1, 3) + "." + text3;
                default:
                    if (text3 == "")
                    {
                        return text2;
                    }

                    return text2 + "." + text3;
            }
        }
    }
}
