﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TimeTableManager.Models;

namespace TimeTableManager
{
    /// <summary>
    /// Interaction logic for LectureDetailWindow.xaml
    /// </summary>
    public partial class LectureDetailWindow : Window

    {
        bool updatebtnclicked = false;
        bool lecidSelected = false;
        bool lecLevelSelected = false;
        MyDbContext dbContext1;
        LecturerDetails NewLecDL = new LecturerDetails();
        LecturerDetails selectedLecturedtls = new LecturerDetails();

        public LectureDetailWindow(MyDbContext dbContext)
        {
            this.dbContext1 = dbContext;
            InitializeComponent();
            GetBuildings();
            getLectureDetai();
            lecidSelected = false;
            lecLevelSelected = false;
            updatebtnclicked = false;
            // AddNewLecturedetails.DataContext = NewLecDL;
        }

        private void GetBuildings()
        {
            CBBuilding.ItemsSource = dbContext1.Buildings.ToList();

        }


        private void getLectureDetai()
        {
            LectureDG.ItemsSource = dbContext1.LectureInformation.Include(r => r.BuildinDSA);


        }

        private void AddLecturerDt(object s, RoutedEventArgs e)
        {


            if (updatebtnclicked)
            {

                if (ValidateInput())
                {

                    if (!(selectedLecturedtls.EmpId.Equals(LecIdName.Text.Trim())) && dbContext1.LectureInformation.Any(r => r.EmpId == LecIdName.Text.Trim()))
                    {
                        new MessageBoxCustom("This Lecture ID Already In the System Use a Different ID", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        return;

                    }



                    selectedLecturedtls.LecName = LecturerName.Text.Trim();

                    selectedLecturedtls.EmpId = LecIdName.Text.Trim();
                    selectedLecturedtls.Faculty = LecturerFaculty.Text.Trim();
                    selectedLecturedtls.Department = LecturerDepartment.Text.Trim();
                    selectedLecturedtls.Center = LecturerCenter.Text.Trim();
                    selectedLecturedtls.BuildinDSA = (Building)CBBuilding.SelectedItem;
                    selectedLecturedtls.EmpLevel = int.Parse(LecLevel.Text);
                    selectedLecturedtls.Rank = LecLevel.SelectedIndex + 1.ToString() + "." + LecIdName.Text.Trim();



                    dbContext1.Update(selectedLecturedtls);

                    dbContext1.SaveChanges();

                    getLectureDetai();


                    if (checksessionhaslectutre(selectedLecturedtls))
                    {
                       
                        sessionConcadingUpdt(selectedLecturedtls.Id, selectedLecturedtls.LecName, LecturerName.Text.Trim());

                    }



                    new MessageBoxCustom("Successfully Updated Lecturer details !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                    Addlecbtn.Content = "Add Lecture";
                    lecidSelected = false;
                    lecLevelSelected = false;
                    updatebtnclicked = false;
                    LecturerName.Text = "";
                    LecIdName.Text = "";
                    LecturerFaculty.Text = "";
                    LecturerDepartment.Text = "";
                    LecturerCenter.Text = "";
                    CBBuilding.Text = "";
                    LecLevel.SelectedIndex = -1;
                    rankDetail.Text = "";


                }
                else
                {
                    new MessageBoxCustom("Please Complete  Lecturer  Details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                }

            }

            else
            {

                if (ValidateInput())
                {


                    //check duplicate lectures
                    if (dbContext1.LectureInformation.Any(b => b.EmpId == LecIdName.Text.Trim()))
                    {
                        new MessageBoxCustom("This Lecture ID Already In the System Use a Different ID", MessageType.Error, MessageButtons.Ok).ShowDialog();
                        return;
                    }


                    NewLecDL.LecName = LecturerName.Text.Trim();
                    NewLecDL.EmpId = LecIdName.Text.Trim();
                    NewLecDL.Faculty = LecturerFaculty.Text.Trim();
                    NewLecDL.Department = LecturerDepartment.Text.Trim();
                    NewLecDL.Center = LecturerCenter.Text.Trim();
                    NewLecDL.BuildinDSA = (Building)CBBuilding.SelectedItem;
                    NewLecDL.EmpLevel = int.Parse(LecLevel.Text.Trim());
                    NewLecDL.Rank = LecLevel.SelectedIndex + 1.ToString() + "." + LecIdName.Text.Trim();
                    Addlecbtn.Content = "Add lecture";
                    dbContext1.LectureInformation.Add(NewLecDL);
                    NewLecDL = new LecturerDetails();


                    dbContext1.SaveChanges();

                    getLectureDetai();







                    new MessageBoxCustom("Successfully Added Lecturer details !", MessageType.Success, MessageButtons.Ok).ShowDialog();


                    lecidSelected = false;
                    lecLevelSelected = false;
                    updatebtnclicked = false;
                    LecturerName.Text = "";
                    LecIdName.Text = "";
                    LecturerFaculty.Text = "";
                    LecturerDepartment.Text = "";
                    LecturerCenter.Text = "";
                    CBBuilding.Text = "";
                    LecLevel.SelectedIndex = -1;
                    rankDetail.Text = "";

                }
                else
                {

                    new MessageBoxCustom("Please Complete  Lecturer  Details correctly !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                }



            }






        }


        private void LecIdSelectChnged(object sender, TextChangedEventArgs e)
        {
            lecidSelected = true;
            if (lecidSelected && lecLevelSelected)
            {
                int lLvl = LecLevel.SelectedIndex + 1;

                rankDetail.Text = lLvl.ToString() + "." + LecIdName.Text.ToString();
                NewLecDL.Rank = rankDetail.Text.ToString();
            }

        }

        private void LecLevel_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            lecLevelSelected = true;

            if (lecidSelected && lecLevelSelected)
            {
                int lLvl = LecLevel.SelectedIndex + 1;

                rankDetail.Text = lLvl.ToString() + "." + LecIdName.Text.ToString();
                NewLecDL.Rank = rankDetail.Text.ToString();

            }
        }


        private void UpdateLectureDetailsIn(object s, RoutedEventArgs e)
        {
            selectedLecturedtls = (s as FrameworkElement).DataContext as LecturerDetails;
            updatebtnclicked = true;
            Addlecbtn.Content = "update lecture";

            LecturerName.Text = selectedLecturedtls.LecName;
            LecIdName.Text = selectedLecturedtls.EmpId;
            LecturerFaculty.Text = selectedLecturedtls.Faculty;
            LecturerDepartment.Text = selectedLecturedtls.Department;
            LecturerCenter.Text = selectedLecturedtls.Center;
            Building buildinDSA = selectedLecturedtls.BuildinDSA;
            CBBuilding.Text = buildinDSA.Name;
            LecLevel.Text = selectedLecturedtls.EmpLevel.ToString();
            rankDetail.Text = selectedLecturedtls.Rank;



        }

        private void DeleteLectureDetailsIn(object s, RoutedEventArgs e)
        {


            bool? Result = new MessageBoxCustom("Are you sure, You want to Delete This Lecture Detail ? ",
              MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

            if (Result.Value)
            {


                var lectureTobeDeleted = (s as FrameworkElement).DataContext as LecturerDetails;

                if (checksessionhaslectutre(lectureTobeDeleted))
                {
                    new MessageBoxCustom("This Lecture Is already assigned Session,Before Delete Lecturer,delete the session", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }

                dbContext1.LectureInformation.Remove(lectureTobeDeleted);
                dbContext1.SaveChanges();
                getLectureDetai();
            }


        }



        private bool ValidateInput()
        {
            if (LecturerName.Text.Trim() == "")
            {
                LecturerName.Focus();
                return false;
            }

            if (LecIdName.Text.Trim() == "")
            {
                LecIdName.Focus();
                return false;
            }
            if (LecIdName.Text.Trim().Length != 6)
            {
                new MessageBoxCustom("Lecture Id Length should be Six !", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                LecIdName.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(LecturerFaculty.Text))
            {
                LecturerFaculty.Focus();
                return false;
            }


            if (string.IsNullOrEmpty(LecturerDepartment.Text))
            {
                LecturerDepartment.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(LecturerCenter.Text))
            {
                LecturerCenter.Focus();
                return false;
            }

            if (CBBuilding.SelectedIndex == -1)
            {
                CBBuilding.Focus();
                return false;
            }


            if (string.IsNullOrEmpty(LecLevel.Text))
            {
                LecLevel.Focus();
                return false;
            }

            return true;
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }


        private void NumberValidationLectureIdd(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private bool checksessionhaslectutre(LecturerDetails LL)
        {
            bool ty = dbContext1.SessionLecturers.Any(r => r.LecturerId == LL.Id);

            return ty;
        }


        public void sessionConcadingUpdt(int lec,String oldname,String nNAme)
        {
            List<Session> sennconcading = dbContext1.SessionLecturers.Where(d => d.Lecdetaiils.Id==lec).Select(s => s.Ssssion).ToList();

            foreach(Session ss in sennconcading)
            {
                // MessageBox.Show("old value" + oldname);
                //  MessageBox.Show("new value" + nNAme); 

                // MessageBox.Show(ss.lecturesLstByConcadinating);
                // str.Replace('e', ' ');
                // MessageBox.Show( ss.lecturesLstByConcadinating.Replace(oldname, nNAme));

                List<LecturerDetails> updtiLects= dbContext1.Sessions .Where(p => p.SessionId == ss.SessionId)
       .SelectMany(r => r.SessionLecturers)
      .Select(rl => rl.Lecdetaiils).ToList();

                ss.lecturesLstByConcadinating = null;
                foreach (LecturerDetails uplec in updtiLects)
                {
                    ss.lecturesLstByConcadinating += uplec.LecName + " ,";

                }

                dbContext1.Update(ss);

                dbContext1.SaveChanges();

            }

        }


    }
}