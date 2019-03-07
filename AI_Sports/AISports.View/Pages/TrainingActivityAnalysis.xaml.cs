﻿using AI_Sports.AISports.Util;
using AI_Sports.Entity;
using AI_Sports.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AI_Sports.AISports.View.Pages
{
    /// <summary>
    /// TrainingActivityAnalysis.xaml 的交互逻辑
    /// </summary>
    public partial class TrainingActivityAnalysis : Page
    {

        TrainingActivityService trainingActivityService = new TrainingActivityService();
        //全局当前课程记录变量
        int? currentCourseCount = 0;


        public TrainingActivityAnalysis()
        {
            InitializeComponent();
            
            

        }
        /// <summary>
        /// 获得从课程分析页面传来的参数 根据传来的课程记录id初始化表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.ExtraData != null)
            {
                currentCourseCount = (int?)e.ExtraData;
                Console.WriteLine("活动分析页收到的currentCourseCount：" + currentCourseCount);
                //传入课程记录id，根据此id查询加载表格
                InitList(currentCourseCount);
            }
           
        }

        /// <summary>
        /// 初始化listview表格
        /// </summary>
        public void InitList(int? currentCourseCount)
        {
            //查询数据
            List<TrainingActivityVO> trainingActivities = trainingActivityService.ListActivityRecords(currentCourseCount);
            //表格加载数据
            CollectionViewSource ListViewGroupSource = (CollectionViewSource)this.FindResource("ListViewGroupSource");
            ListViewGroupSource.Source = trainingActivities;
            

            


        }
        /// <summary>
        /// 点击后退按钮 退到训练课程分析页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new Uri("/AI_Sports;component/AISports.View/Pages/TrainingCourseAnalysis.xaml", UriKind.Relative));

        }

        private void Speech_Click(object sender, RoutedEventArgs e)
        {
            List<TrainingActivityVO> trainingActivities = trainingActivityService.ListActivityRecords(currentCourseCount);
            int? strengthActivityCount = 0;
            int? enduranceActivityCount = 0;

            foreach (var item in trainingActivities)
            {
                if ("0".Equals(item.Activity_type))
                {
                    //力量循环加一
                    strengthActivityCount++;
                }
                else if ("1".Equals(item.Activity_type))
                {
                    //力量耐力循环加一
                    enduranceActivityCount++;
                }
            }

            StringBuilder speechText = new StringBuilder();
            speechText.Append("您当前查看的是第");
            speechText.Append(currentCourseCount);
            speechText.Append("次课程记录。");
            speechText.Append("总共进行了");
            speechText.Append(trainingActivities.Count);
            speechText.Append("轮训练活动，其中力量循环");
            speechText.Append(strengthActivityCount);
            speechText.Append("轮，");
            speechText.Append("力量耐力循环");
            speechText.Append(enduranceActivityCount);
            speechText.Append("轮。您可以点击训练活动展开查看详细记录。");
            SpeechUtil.read(speechText.ToString());

        }

        ///按钮绑定测试
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Controls.Button btn = (System.Windows.Controls.Button)sender;
        //    System.Windows.MessageBox.Show("当前行的设备类型：" + btn.Tag.ToString());
        //}
    }
}
