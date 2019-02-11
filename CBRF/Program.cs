using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.IO;
//using System.Net;
//using System.Net.Sockets;
using System.Xml;

namespace CBRF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Курсы валют к рублю:");
            Console.WriteLine(cbrf());
            Console.WriteLine("Нажмите Enter ");
            Console.ReadLine();
        }

        private static string cbrf()
        {
            string cbrf = ""; //В эту переменную накапливаем информацию о валютах
            //Console.Write("Load xml...");
            XmlTextReader reader = new XmlTextReader("http://www.cbr.ru/scripts/XML_daily.asp");
            //Перебираем все узлы в загруженном документе
            //Console.WriteLine(" Done");
            while (reader.Read())
            {
                //Console.WriteLine("Read...");
                //Проверяем тип текущего узла
                switch (reader.NodeType)
                {
                    //Если этого элемент Valute, то начинаем анализировать атрибуты
                    case XmlNodeType.Element:
                        if (reader.Name == "Valute")
                        {
                            if (reader.HasAttributes)
                            {
                                //Метод передвигает указатель к следующему атрибуту
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "ID")
                                    {
                                        //Console.Write(reader.Value);
                                        //Если значение атрибута равно R01235, то перед нами информация о курсе доллара
                                        //Возвращаемся к элементу, содержащий текущий узел атрибута
                                        reader.MoveToElement();
                                        //Считываем содержимое дочерних узлов
                                        string USDXml = reader.ReadOuterXml();
                                        //Из выдернутых кусков XML кода создаем новые XML документы
                                        XmlDocument usdXmlDocument = new XmlDocument();
                                        usdXmlDocument.LoadXml(USDXml);
                                        //Начинаем парсить каждую валюту

                                        //Парсим код валюты
                                        XmlNode xmlCode= usdXmlDocument.SelectSingleNode("Valute/CharCode");
                                        //Парсим номинал
                                        XmlNode xmlNominal = usdXmlDocument.SelectSingleNode("Valute/Nominal");
                                        //Парсим название
                                        XmlNode xmlName = usdXmlDocument.SelectSingleNode("Valute/Name");
                                        //Парсим курс и конвертируем в decimal
                                        XmlNode xmlKurs = usdXmlDocument.SelectSingleNode("Valute/Value");
                                        decimal xValue = Convert.ToDecimal(xmlKurs.InnerText);
                                        string addValute= xmlCode.InnerText.ToString() + " \t" + xmlNominal.InnerText.ToString() + " " + xmlName.InnerText.ToString() + " = " + xValue.ToString() + "\n";
                                        //Console.WriteLine(addValute);
                                        cbrf = cbrf + addValute;
                                    }
                                }
                            }
                        }
                        //Console.WriteLine("break this");
                    break;

                }
            }
            //string cbrf = "USD:" + usdValue.ToString() + " EUR:" + euroValue.ToString();
            //logBot(cbrf);
            return (cbrf);
        }


    }
}
