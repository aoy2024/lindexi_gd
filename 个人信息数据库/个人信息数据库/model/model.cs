﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace 个人信息数据库.model
{
    public class model : notify_property
    {
        public model()
        {
            ran = new Random();
        }
        Random ran
        {
            set;
            get;
        }
        /// <summary>
        /// 数据库ip
        /// </summary>
        public string DataSource
        {
            set;
            get;
        } = "QQLINDEXI\\SQLEXPRESS";
        /// <summary>
        /// 数据库名
        /// </summary>
        public string InitialCatalog
        {
            set;
            get;
        } = "grxx";
        /// <summary>
        /// 连接数据库
        /// </summary>
        public string connect
        {
            set
            {
                value = string.Empty;
            }
            get
            {
                return $"Data Source={DataSource};Initial Catalog={InitialCatalog};Integrated Security=True";
            }
        }

        public void ce()
        {
            //List<caddressBook> addressBook = lajiaddressBook();
            //writeaddressBook(addressBook);

        }

        public ObservableCollection<caddressBook> newaddressBook()
        {
            string addressBookname = "temp";
            string sqlAddressBook = $"{usesql}{line}SELECT * FROM {addressBookname};";
            ObservableCollection<caddressBook> addressBook = new ObservableCollection<caddressBook>();
            string id = "id";
            string name = "name";
            string contact = "contact";
            string naddress = "naddress";
            string city = "city";
            string comment = "comment";
            using (SqlConnection sql = new SqlConnection(connect))
            {
                sql.Open();
                using (SqlCommand cmd = new SqlCommand(sqlAddressBook , sql))
                {
                    using (SqlDataReader read = cmd.ExecuteReader())
                    {
                        //判断当前的reader是否读取到了数据
                        if (read.HasRows)
                        {
                            int idindex = read.GetOrdinal(id);
                            int nameindex = read.GetOrdinal(name);
                            int contactindex = read.GetOrdinal(contact);
                            int naddressindex = read.GetOrdinal(naddress);
                            int cityindex = read.GetOrdinal(city);
                            int commentindex = read.GetOrdinal(comment);
                            while (read.Read())
                            {
                                caddressBook temp = new caddressBook();
                                temp.id = read.GetInt32(idindex).ToString();
                                temp.name = read.GetString(nameindex).Trim();
                                temp.contact = read.GetString(contactindex).Trim();
                                temp.address = read.GetString(nameindex).Trim();
                                temp.city = read.GetString(cityindex).Trim();
                                temp.comment = read.GetString(commentindex).Trim();

                                addressBook.Add(temp);
                            }                           
                        }
                    }
                }
            }
            return addressBook;
        }


        /// <summary>
        /// 写入通讯录
        /// </summary>
        /// <param name="addressBook"></param>
        public void writeaddressBook(List<caddressBook> addressBook)
        {
            string t = "temp";
            string strsql;       
            foreach (var temp in addressBook)
            {
                strsql = $"{usesql}{line}insert into {t}(id,name,contact,naddress,city,comment){line}values('{temp.id}','{temp.name}','{temp.contact}','{temp.address}','{temp.city}','{temp.comment}');{line}";
                write(strsql);
            }
        }

        /// <summary>
        /// 写数据
        /// </summary>
        /// <param name="strsql"></param>
        public void write(string strsql)
        {
            using (SqlConnection sql = new SqlConnection(connect))
            {
                sql.Open();
                using (SqlCommand cmd = new SqlCommand(strsql , sql))
                {
                    //字符串比char(10)长 将截断字符串或二进制数据
                    int r = cmd.ExecuteNonQuery();
                }
            }
        }


        //public void ce()
        //{
        //    string inputJsonString = @"
        //        [
        //            {StudentID:'100',Name:'aaa',Hometown:'china'},
        //            {StudentID:'101',Name:'bbb',Hometown:'us'},
        //            {StudentID:'102',Name:'ccc',Hometown:'england'}
        //        ]";
        //    JArray jsonObj = JArray.Parse(inputJsonString);
        //    string message = @"<table border='1'>
        //            <tr><td width='80'>StudentID</td><td width='100'>Name</td><td width='100'>Hometown</td></tr>";
        //    string tpl = "<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
        //    foreach (JObject jObject in jsonObj)
        //    {
        //        message += String.Format(tpl , jObject["StudentID"] , jObject["Name"] , jObject["Hometown"]);
        //    }
        //    message += "</table>";
        //    //lbMsg.InnerHtml = message;
        //}

        private string ranstr(int n)
        {
            StringBuilder str = new StringBuilder();
            int[] 中文 = new int[2] { 19968 , 40895 };
            for (int i = 0; i < n; i++)
            {
                str.Append(Convert.ToChar(ran.Next(中文[0] , 中文[1])));
            }
            return str.ToString();
        }

        private string line
        {
            set;
            get;
        } = "\n";
        string usesql
        {
            set
            {
                value = string.Empty;
            }
            get
            {
                return $"use {InitialCatalog}";
            }
        }

        private List<caddressBook> lajiaddressBook()
        {
            List<caddressBook> addressBook = new List<caddressBook>();
            List<string> chinacity = new List<string>();
            chinacity.AddRange(sql.城市.Split(new char[2] { '\r' , '\n' }));

            for (int i = 0; i < chinacity.Count; i++)
            {
                if (string.IsNullOrEmpty(chinacity[i]))
                {
                    chinacity.RemoveAt(i);
                    i--;
                }
                else
                {
                    chinacity[i] = chinacity[i].Trim();
                }
            }

            int n = 100;
            caddressBook temp;

            for (int i = 0; i < n; i++)
            {
                temp = new caddressBook()
                {
                    id = i.ToString() ,
                    name = ranstr(3) ,
                    contact = ran.Next().ToString() ,
                    address = chinacity[ran.Next(chinacity.Count)] ,
                    city = chinacity[ran.Next(chinacity.Count)] ,
                    comment = "随机的名，作为测试"
                };
                addressBook.Add(temp);
            }
            return addressBook;
        }
    }
}
