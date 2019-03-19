﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Platform.WinRT;
using SQLite.Net.Interop;
using static System.Diagnostics.Debug;
using Windows.Storage;
using TestEntity;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace SDKTemplate
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Test : Page
    {
        public Test()
        {
            this.InitializeComponent();
            dbname = "test.db";
        }

        public string dbname;

        //创建表
        private void OnClick(object sender, RoutedEventArgs e)
        {
            string fdlocal = ApplicationData.Current.LocalFolder.Path;
            string filename = dbname;
            string dbfullpath = Path.Combine(fdlocal, filename);

            ISQLitePlatform platform = new SQLitePlatformWinRT();
            // 连接对象
            SQLiteConnection conn = new SQLiteConnection(platform, dbfullpath);
            WriteLine("db pathe: " + conn.DatabasePath);

            // 创建表
            int rn = conn.CreateTable<Person>(CreateFlags.None);
            WriteLine("create table res = {0}", rn);

            conn.Dispose();
        }

        //插入数据
        private void OnInsert(object sender, RoutedEventArgs e)
        {
            string localFolder = ApplicationData.Current.LocalFolder.Path;
            string dbFullPath = Path.Combine(localFolder, dbname);
            // 建立连接
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbFullPath))
            {
                //conn.DeleteAll<Person>();
                // 插入数据
                Person[] stus =
                {
                    new Person { Name="小王",Age = 21,Gender = "male" },
                    new Person { Name = "小赵",Age=30,Gender = "male" },
                    new Person {Name="小丁",Age=25,Gender = "male" },
                    new Person {Name="小马",Age=27,Gender = "female" },
                    new Person {Name="小陈",Gender = "male"}
                };
                int n = conn.InsertAll(stus);
                WriteLine($"已插入 {n} 条数据。");
            }
        }

        //读取数据
        private void OnReadData(object sender, RoutedEventArgs e)
        {
            string localFolderPath = ApplicationData.Current.LocalFolder.Path;
            string dbFullpath = Path.Combine(localFolderPath, dbname);

            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbFullpath))
            {
                // 获取列表
                TableQuery<Person> t = conn.Table<Person>();
                var q = from s in t.AsParallel<Person>()
                        orderby s.ID
                        select s;
                // 绑定

                lv.ItemsSource = q;
            }
        }

        //删除数据
        private void OnDeleteData(object sender, RoutedEventArgs e)
        {
            string localFolder = ApplicationData.Current.LocalFolder.Path;
            string dbFullPath = Path.Combine(localFolder, dbname);
            // 建立连接
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbFullPath))
            {
                conn.DeleteAll<Person>();
                TableQuery<Person> t = conn.Table<Person>();
                var q = from s in t.AsParallel<Person>()
                        orderby s.ID
                        select s;
                // 绑定

                lv.ItemsSource = q;
            }
        }
    }
}