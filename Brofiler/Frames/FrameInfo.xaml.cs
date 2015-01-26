﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using Profiler.Data;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace Profiler
{
	/// <summary>
	/// Interaction logic for FrameInfo.xaml
	/// </summary>
	public partial class FrameInfo : UserControl
	{
		public FrameInfo()
		{
			this.InitializeComponent();
      SummaryTable.FilterApplied += new ApplyFilterEventHandler(ApplyFilterToEventTree);
		}

    private Data.Frame frame;

    public void SetFrame(Data.Frame frame)
    {
      this.frame = frame;
      Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { frame.Load();  this.DataContext = frame; }));
    }

    public void RefreshFilter(object sender, RoutedEventArgs e)
    {
      SummaryTable.RefreshFilter();
    }

    private void ApplyFilterToEventTree(HashSet<Object> filter, FilterMode mode)
    {
			if (ShowAllFunctions.IsChecked ?? true)
				mode.ShowAll = true;

			if (FilterByTime.IsChecked ?? true)
			{
				double limit = 0.0;
				if (Double.TryParse(TimeLimit.Text.Replace('.', ','), out limit))
					mode.TimeLimit = limit;
			}

      //ThreadStart thread = new ThreadStart(delegate ()
      {
        //Stopwatch stop = new Stopwatch();
        //stop.Start();

        HashSet<Object> roof = new HashSet<Object>();

        foreach (Object node in filter)
        {
          BaseTreeNode current = (node as BaseTreeNode).Parent;
          while (current != null)
          {
            if (!roof.Add(current))
              break;

            current = current.Parent;
          }
        }

        foreach (var node in EventTreeView.ItemsSource)
        {
          if (node is BaseTreeNode)
          {
            BaseTreeNode eventNode = node as BaseTreeNode;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
              eventNode.ApplyFilter(roof, filter, mode);
            }), DispatcherPriority.Loaded);
          }
        }
        //stop.Stop();
        //MessageBox.Show(stop.ElapsedMilliseconds.ToString());
      }
      //);

      //new Thread(thread).Start();
    }

		private void OnTreeViewItemMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (e.Source is FrameworkElement)
			{
				e.Handled = true;

				FrameworkElement item = e.Source as FrameworkElement;

				Application.Current.Dispatcher.Invoke(new Action(() =>
				{
					Object windowDataContext = null;
					if (item.DataContext is SamplingNode)
						windowDataContext = SourceView<SamplingBoardItem, SamplingDescription, SamplingNode>.Create(SummaryTable.DataContext as Board<SamplingBoardItem, SamplingDescription, SamplingNode>, (item.DataContext as SamplingNode).Description.Path);
					else if (item.DataContext is EventNode)
						windowDataContext = SourceView<EventBoardItem, EventDescription, EventNode>.Create(SummaryTable.DataContext as Board<EventBoardItem, EventDescription, EventNode>, (item.DataContext as EventNode).Description.Path);

					if (windowDataContext != null)
					{
						new SourceWindow() { DataContext = windowDataContext, Owner = Application.Current.MainWindow }.Show();
					}
				}));
			}
		}
	}
}