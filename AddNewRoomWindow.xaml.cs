﻿using System;
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
    /// Interaction logic for AddNewRoomWindow.xaml
    /// </summary>
    public partial class AddNewRoomWindow : Window
    {
        MyDbContext dbContext1;
        Building SeletedBuilding = new Building();
        Room RoomToEdit = new Room();

        public AddNewRoomWindow(MyDbContext dbContext, Room room)
        {
            RoomToEdit = room;
            dbContext1 = dbContext;
            InitializeComponent();
            GetBuildings();

            if (RoomToEdit != null)
            {

                EnableUpdateMode();
            }
        }

        private void GetBuildings()
        {
            CBBuilding.ItemsSource = dbContext1.Buildings.ToList();

        }

        private void EnableUpdateMode()
        {
            TxtTitle.Content = "Change Room Details";
            TxtRid.Text = RoomToEdit.Rid;
            TxtCapacity.Text = RoomToEdit.Capacity.ToString();
            CBBuilding.SelectedItem = RoomToEdit.BuildingAS;
            CBType.Text = RoomToEdit.Type;

            BtnSave.Content = "Update";
            BtnSave.Click -= AddRoom;
            BtnSave.Click += UpdateRoom;


        }

        private void AddRoom(Object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                Room NewRoom = new Room();

                NewRoom.Rid = TxtRid.Text;
                NewRoom.Capacity = int.Parse(TxtCapacity.Text);
                NewRoom.BuildingAS = (Building)CBBuilding.SelectedItem;
                NewRoom.Type = CBType.Text;

                //insert that object to database
                if (dbContext1.Rooms.Any(r => r.Rid == TxtRid.Text))
                {
                    new MessageBoxCustom("This ID Already In the System Use a Different ID", MessageType.Error, MessageButtons.Ok).ShowDialog();

                }
                else
                {

                    dbContext1.Rooms.Add(NewRoom);
                    dbContext1.SaveChanges();
                    new MessageBoxCustom("Successfully Added to the System !", MessageType.Success, MessageButtons.Ok).ShowDialog();

                    TxtCapacity.Text = "";
                    TxtRid.Text = "";
                    CBBuilding.SelectedIndex = -1;
                    CBType.SelectedIndex = -1;


                }
            }
            else
            {
                new MessageBoxCustom("Please Complete Room Details to Continue !", MessageType.Warning, MessageButtons.Ok).ShowDialog();

            }

        }

        private void UpdateRoom(Object s, RoutedEventArgs e)
        {
            if (ValidateInput())
            {


                //insert that object to database
                if (!(RoomToEdit.Rid.Equals(TxtRid.Text.Trim())) && dbContext1.Rooms.Any(r => r.Rid == TxtRid.Text))
                {
                    new MessageBoxCustom("This ID Already In the System Use a Different ID", MessageType.Error, MessageButtons.Ok).ShowDialog();

                }
                else
                {
                    RoomToEdit.Rid = TxtRid.Text;
                    RoomToEdit.Type = CBType.Text;
                    RoomToEdit.BuildingAS = (Building)CBBuilding.SelectedItem;
                    RoomToEdit.Capacity = int.Parse(TxtCapacity.Text);
                    //  var query = $"UPDATE Buildings SET Id = '{txtId.Text}', Name = '{txtName.Text}' WHERE Id='{SeletedBuilding.Id}'";
                    //  BuildingDG.SelectedItem = NewBuilding;

                    // dbContext1.Database.ExecuteSqlRaw(query);
                    dbContext1.Rooms.Update(RoomToEdit);
                    dbContext1.SaveChanges();
                    this.Close();

                }


            }
            else
            {
                new MessageBoxCustom("Please Complete Room Details to Continue !", MessageType.Warning, MessageButtons.Ok).ShowDialog();

            }
        }

        private void CloseWindow(Object s, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (TxtRid.Text.Trim() == "")
            {
                TxtRid.Focus();
                return false;
            }

            if (TxtCapacity.Text.Trim() == "")
            {
                TxtCapacity.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(CBType.Text))
            {
                CBType.Focus();
                return false;
            }

            if (CBBuilding.SelectedIndex == -1)
            {
                CBBuilding.Focus();
                return false;
            }

            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GoBack(Object s, RoutedEventArgs e)
        {
    
            this.Close();

        }
    }
}