﻿using KDSingleManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
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

namespace KDSingleManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static AppContext _context = new AppContext();
        private CollectionViewSource subconViewSource;

        public MainWindow()
        {
            InitializeComponent();
            subconViewSource = (CollectionViewSource)FindResource(nameof(subconViewSource));
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
            _context.Subcontractors.Load();
            _context.SaveChanges();
            subconViewSource.Source = _context.Subcontractors.Local.ToObservableCollection();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _context.Dispose();
        }

        private void btn_OpenPage_Click(object sender, RoutedEventArgs e)
        {
            WinNewSubcontractor wns = new WinNewSubcontractor();
            wns.Show();
        }
        private void dg_SubconList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var s = (Subcontractor)dg_SubconList.SelectedItem;
            WinNewSubcontractor wns = new WinNewSubcontractor(s);
            wns.Show();
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dg_SubconList.SelectedItem != null)
            {
                Subcontractor w = (Subcontractor)dg_SubconList.SelectedItem;
                _context.Subcontractors.Remove(w);
                _context.SaveChanges();
                subconViewSource.Source = _context.Subcontractors.Local.ToObservableCollection();
            }
        }

        //private void dg_SubconList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var x = (Subcontractor)dg_SubconList.SelectedItem;
        //    MessageBox.Show(x.FirstName + " " + x.LastName);
        //}

    }
}