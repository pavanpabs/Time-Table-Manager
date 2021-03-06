﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeTableManager.Models;

namespace TimeTableManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyDbContext dbContext1;
        public MainWindow(MyDbContext dbContext)
        {
            InitializeComponent();
            dbContext1 = dbContext;
        }

        public void buttonclick1(object sender, RoutedEventArgs e)
        {

        }

        private void LaunchStudents(Object s, RoutedEventArgs e)
        {
            StudentWindow studentWindow = new StudentWindow(dbContext1);
            studentWindow.Show();
            this.Close();


        }

        private void LaunchTags(Object s, RoutedEventArgs e)
        {
            TagWindow tagWindow = new TagWindow(dbContext1);
            tagWindow.Show();
            this.Close();


        }

        private void LaunchLecturers(Object s, RoutedEventArgs e)
        {
            // Collection which will take your ObservableCollection


        }

        private void LaunchSubjects(Object s, RoutedEventArgs e)
        {
          
            SubjectDetailsWindow locationsWindow = new SubjectDetailsWindow(dbContext1);
            locationsWindow.Show();
            this.Close();

        }

        private void LaunchLocations(Object s, RoutedEventArgs e)
        {
            // Collection which will take your ObservableCollection
            LocationsWindow locationsWindow = new LocationsWindow(dbContext1);
            locationsWindow.Show();
            this.Close();

        }

        private void LaunchStats(Object s, RoutedEventArgs e)
        {
            // Collection which will take your ObservableCollection
            StatisticsWindow locationsWindow = new StatisticsWindow(dbContext1);
            locationsWindow.Show();
            this.Close();

        }

        private void LaunchLectureManagement(Object s, RoutedEventArgs e)
        {
            // Collection which will take your ObservableCollection
            LectureDetailWindow locationsWindow = new LectureDetailWindow(dbContext1);
            locationsWindow.Show();
            this.Close();

        }

        private void OpenSchedule(object sender, RoutedEventArgs e)
        {
          
            ScheduleWindow sw = new ScheduleWindow(dbContext1);
            sw.Show();
            this.Close();
        }

        private void LaunchNotAvailableTime(Object s, RoutedEventArgs e)
        {

            NotAvailableDetailsWindow NotAvailableDetailsWindow = new NotAvailableDetailsWindow(dbContext1);
            NotAvailableDetailsWindow.Show();
            this.Close();


        }

        private void LaunchConsecutiveSessions(Object s, RoutedEventArgs e)
        {

            ConsecutiveSessionsWindow consecutiveSessions = new ConsecutiveSessionsWindow(dbContext1);
            consecutiveSessions.Show();
            this.Close();


        }

        private void LaunchParallelSessions(Object s, RoutedEventArgs e)
        {

            ParallelSessionsWindow parallelSessions = new ParallelSessionsWindow(dbContext1);
            parallelSessions.Show();
            this.Close();


        }

        private void OpenGenerate(object sender, RoutedEventArgs e)
        {

            GenerateWindow sw = new GenerateWindow(dbContext1);
            sw.Show();
            this.Close();
        }
      

    private void Sessionmanagement(object sender, RoutedEventArgs e)
        {

            SessionWindow sw = new SessionWindow(dbContext1);
            sw.Show();
            this.Close();
        }




    }

}
