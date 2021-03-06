﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for LocationsWindow.xaml
    /// </summary>
    public partial class LocationsWindow : Window
    {
        MyDbContext dbContext1;
        Room SelectedRoom = null;
        ICollection<Room> Rooms;
        public ObservableCollection<Room> RoomCollection { get; set; }

        public LocationsWindow(MyDbContext dbContext)
        {
            InitializeComponent();
            this.dbContext1 = dbContext;
            GetRooms();
            GetBuildings();
            LoadOthers();


        }

        private void GetRooms()
        {
            if (Rooms != null)
            {
                Rooms.Clear();
            }
            Rooms = dbContext1.Rooms.Include(r => r.BuildingAS).ToList();

            RoomsDG.ItemsSource = new ObservableCollection<Room>(Rooms);

        }

        private void GetBuildings()
        {
            CBSearchBuilding.ItemsSource = dbContext1.Buildings.ToList();

        }


        private void LoadOthers()
        {
            CBSearchSes.ItemsSource = dbContext1.Sessions
                     .Include(r => r.subjectDSA)
                     .Include(r => r.tagDSA)
                     .Include(r => r.Room).ToList();

            CBSearchLect.ItemsSource = dbContext1.LectureInformation.ToList();

            CBSearchSub.ItemsSource = dbContext1.SubjectInformation.ToList();

            List<String> data1 = dbContext1.Students.Select(r => r.groupId).Distinct().ToList();
            List<String> data = dbContext1.Students.Select(r => r.subGroupId).Distinct().ToList();

            foreach (String s in data)
            {
                data1.Add(s);
            }

            CBSearchGRP.ItemsSource = data1;
        }

        public void AddNewRoom(Object s, RoutedEventArgs e)
        {

            AddNewRoomWindow addNewRoomWindow = new AddNewRoomWindow(dbContext1, null);
            addNewRoomWindow.Closed += AddClosed;
            addNewRoomWindow.ShowDialog();

        }

        public void UpdateRoom(Object s, RoutedEventArgs e)
        {
            if (SelectedRoom == null)
            {

                new MessageBoxCustom("Please Select A Room from the table before Updating!",
                   MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
            else
            {
                AddNewRoomWindow addNewRoomWindow = new AddNewRoomWindow(dbContext1, SelectedRoom);
                addNewRoomWindow.Closed += AddClosed;
                addNewRoomWindow.ShowDialog();
            }
        }

        public void DeleteRoom(Object s, RoutedEventArgs e)
        {
            //insert that object to database
            if (SelectedRoom == null)
            {
                new MessageBoxCustom("Please Select a Room to Delete", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
            else
            {
                //MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                //if (messageBoxResult == MessageBoxResult.Yes)
                //{
                //    dbContext1.Rooms.Remove(SelectedRoom);
                //    dbContext1.SaveChanges();
                //    GetRooms();
                //}

                bool? Result = new MessageBoxCustom("Are you sure, You want to Delete This Room ? ",
                    MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();

                if (Result.Value)
                {
                    dbContext1.Rooms.Remove(SelectedRoom);
                    dbContext1.SaveChanges();
                    GetRooms();
                    SelectedRoom = null;
                }


            }


        }
        private void GoBack(Object s, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(dbContext1);
            mainWindow.Show();
            this.Close();

        }


        public void ManageBuilding(Object s, RoutedEventArgs e)
        {
            BuildingsWindow buildingsWindow = new BuildingsWindow(dbContext1);
            buildingsWindow.Closed += AddClosed;
            buildingsWindow.ShowDialog();
        }

        private void UpdateSelection(Object s, RoutedEventArgs e)
        {


            if (RoomsDG.SelectedItem != null)
            {
                try
                {
                    SelectedRoom = (Room)RoomsDG.SelectedItem;
                    TxtBuilding.Text = SelectedRoom.BuildingAS.Name;
                    TxtCapacity.Text = SelectedRoom.Capacity.ToString();
                    TxtRid.Text = SelectedRoom.Rid;
                    TxtType.Text = SelectedRoom.Type;

                    //load lecturers
                    LVlecturer.ItemsSource = dbContext1.Rooms
                    .Where(p => p.Id == SelectedRoom.Id)
                    .SelectMany(r => r.RoomLecturers)
                    .Select(rl => rl.Lecturer).ToList();

                    //load subjects
                    LVSubjects.ItemsSource = dbContext1.Rooms
                    .Where(p => p.Id == SelectedRoom.Id)
                    .SelectMany(r => r.RoomSubjects)
                    .Select(rl => rl.Subject).ToList();

                    //load nats
                    LVNAT.ItemsSource = dbContext1.RoomNATs
                    .Where(r => r.room.Id == SelectedRoom.Id)
                    .ToList();

                    //load sessions
                    SessionDG.ItemsSource = dbContext1.Sessions
                    .Where(p => p.Room.Id == SelectedRoom.Id)
                    .Include(r => r.subjectDSA)
                    .Include(r => r.tagDSA).ToList();

                    //load groups
                    LVGroups.ItemsSource = dbContext1.RoomGroups
                    .Where(p => p.room.Id == SelectedRoom.Id).ToList();
                }
                catch (Exception ex)
                {
                    SelectedRoom = null;
                }
            }
            else
            {
                TxtBuilding.Text = "";
                TxtCapacity.Text = "";
                TxtRid.Text = "";
                TxtType.Text = "";

                LVGroups.ItemsSource = null;
                LVlecturer.ItemsSource = null;
                LVNAT.ItemsSource = null;
                LVSubjects.ItemsSource = null;
                SessionDG.ItemsSource = null;
            }



        }

        public void AddClosed(object sender, System.EventArgs e)
        {
            //This gets fired off
            GetRooms();
            GetBuildings();

        }

        private void SerachById(Object s, RoutedEventArgs e)
        {
            // Collection which will take your ObservableCollection
            var _itemSourceList = new CollectionViewSource() { Source = Rooms };

            // ICollectionView the View/UI part 
            ICollectionView Itemlist = _itemSourceList.View;

            // your Filter
            var yourCostumFilter = new Predicate<object>(item => ((Room)item).Rid.ToLower().Contains(txtSearchId.Text.ToLower()));

            //now we add our Filter
            Itemlist.Filter = yourCostumFilter;

            RoomsDG.ItemsSource = Itemlist;

        }


        private void SearchByType(Object s, RoutedEventArgs e)
        {
            if (CBSearchType.SelectedItem != null)
            {
                // Collection which will take your ObservableCollection
                var _itemSourceList = new CollectionViewSource() { Source = Rooms };

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;

                // your Filter
                var yourCostumFilter = new Predicate<object>(item => !((Room)item).Type.Contains(CBSearchType.Text.ToString()));

                //now we add our Filter
                Itemlist.Filter = yourCostumFilter;

                RoomsDG.ItemsSource = Itemlist;
            }
            else
            {
                RoomsDG.ItemsSource = Rooms;
            }
        }

        private void SearchByBuilding(Object s, RoutedEventArgs e)

        {
            if (CBSearchBuilding.SelectedItem != null)
            {
                // Collection which will take your ObservableCollection
                var _itemSourceList = new CollectionViewSource() { Source = Rooms };

                // ICollectionView the View/UI part 
                ICollectionView Itemlist = _itemSourceList.View;

                // your Filter
                var yourCostumFilter = new Predicate<object>(item => ((Room)item).BuildingAS == CBSearchBuilding.SelectedItem);

                //now we add our Filter
                Itemlist.Filter = yourCostumFilter;

                RoomsDG.ItemsSource = Itemlist;
            }
            else
            {
                RoomsDG.ItemsSource = Rooms;
            }


        }

        private void SearchBySession(Object s, RoutedEventArgs e)

        {
            if (CBSearchSes.SelectedItem != null)
            {
                Session selectedSession = (Session)CBSearchSes.SelectedItem;
                CBSearchLect.SelectedItem = null;

                try
                {
                    RoomsDG.Items.Clear();

                    if (selectedSession.Room != null)
                    {
                        RoomsDG.Items.Add(selectedSession.Room);
                        RoomsDG.Items.Refresh();
                    }

                }
                catch (Exception ex)
                {
                    RoomsDG.ItemsSource = null;
                }
            }
            else
            {
                RoomsDG.Items.Clear();
                RoomsDG.ItemsSource = Rooms;
            }


        }


        private void SearchByLecturer(Object s, RoutedEventArgs e)
        {
            if (CBSearchLect.SelectedItem != null)
            {
                CBSearchSub.SelectedItem = null;
                CBSearchSes.SelectedItem = null;
                CBSearchGRP.SelectedItem = null;
               
                LecturerDetails selectedLec = (LecturerDetails)CBSearchLect.SelectedItem;


                RoomsDG.ItemsSource = dbContext1.RoomLecturers
                    .Where(r => r.Lecturer.Id == selectedLec.Id).Include(r => r.Room)
                    .Select(r => r.Room).ToList();



            }
            else
            {
                RoomsDG.ItemsSource = Rooms;
            }


        }

        private void SearchBySubject(Object s, RoutedEventArgs e)
        {
            if (CBSearchSub.SelectedItem != null)
            {
                CBSearchGRP.SelectedItem = null;
                CBSearchSes.SelectedItem = null;
                CBSearchLect.SelectedItem = null;

                SubjectDetails selectedSub = (SubjectDetails)CBSearchSub.SelectedItem;


                RoomsDG.ItemsSource = dbContext1.RoomSubjects
                    .Where(r => r.Subject.Id == selectedSub.Id)
                    .Include(r => r.Room)
                    .Select(r => r.Room).ToList();



            }
            else
            {
                RoomsDG.ItemsSource = Rooms;
            }


        }

        private void SearchByGRP(Object s, RoutedEventArgs e)
        {
            if (CBSearchGRP.SelectedItem != null)
            {
                CBSearchSub.SelectedItem = null;
                CBSearchSes.SelectedItem = null;
                CBSearchLect.SelectedItem = null;

                String selectedGRP = CBSearchGRP.Text;


                RoomsDG.ItemsSource = dbContext1.RoomGroups
                    .Where(r => r.Group.Equals(selectedGRP.ToString()))
                    .Include(r => r.room)
                    .Select(r => r.room).ToList();



            }
            else
            {
                RoomsDG.ItemsSource = Rooms;
            }


        }



    }
}
