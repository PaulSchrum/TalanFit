using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;
using System.Timers;

namespace TalanFit
{
   public class WiiBalanceBoardViewModel : INotifyPropertyChanged
   {
      private WiimoteCollection allWiimoteConnections = new WiimoteCollection();
      public Wiimote theBalanceBoard = new Wiimote();
      private Timer pollingTimer = new Timer(100);

      public WiiBalanceBoardViewModel()
      {
         BoardIsConnected = false;
         try
         {
            allWiimoteConnections.FindAllWiimotes();
         }
         catch(Exception e)
         {
            BoardIsConnected = false;
            return;
         }
         try
         {
            theBalanceBoard = allWiimoteConnections[0];
            theBalanceBoard.Connect();
         }
         catch (Exception WiimoteNotFoundException)
         {
            BoardIsConnected = false;
            return;
         }
         //CenterOfGravity = new PointF_Replica();
         CenterOfGravity_x = 15.0F; //CenterOfGravity.Y = 16.4F;
         //theBalanceBoard.WiimoteChanged += balanceBoard_Changed;

         BoardIsConnected = true;
         pollingTimer.Elapsed += new ElapsedEventHandler(pollTheBalanceBoard);
         pollingTimer.Enabled = true;
      }

      private void pollTheBalanceBoard(object source, ElapsedEventArgs e)
      {
         Weight_Lb = String.Format("{0:0.0}", 
            theBalanceBoard.WiimoteState.BalanceBoardState.WeightLb);
         String test = String.Format("{0:0.00}", theBalanceBoard.WiimoteState.BalanceBoardState.CenterOfGravity.X);
         CenterOfGravity_x = theBalanceBoard.WiimoteState.BalanceBoardState.CenterOfGravity.X;
         CenterOfGravity_y = theBalanceBoard.WiimoteState.BalanceBoardState.CenterOfGravity.Y;
      }

      private String cogX_string_;
      public String CogXstring
      {
         get { return cogX_string_; }
         set
         {
            cogX_string_ = value;
            RaisePropertyChanged("CogXstring");
         }
      }

      private String cogY_string_;
      public String CogYstring
      {
         get { return cogY_string_; }
         set
         {
            cogY_string_ = value;
            RaisePropertyChanged("CogYstring");
         }
      }

      private Single centerOfGravity_x_;
      public Single CenterOfGravity_x
      {
         get { return centerOfGravity_x_; }
         set
         {
            centerOfGravity_x_ = value;
            RaisePropertyChanged("CenterOfGravity_x");
         }
      }

      private Single centerOfGravity_y_;
      public Single CenterOfGravity_y
      {
         get { return centerOfGravity_y_; }
         set
         {
            centerOfGravity_y_ = value;
            RaisePropertyChanged("CenterOfGravity_y");
         }
      }

      private String weight_lb_;
      public String Weight_Lb
      {
         get { return weight_lb_; }
         set
         {
            weight_lb_ = value;
            RaisePropertyChanged("Weight_Lb");
         }
      }

      private Boolean boardIsConnected_;
      private Boolean BoardIsConnected
      {
         get { return boardIsConnected_; }
         set
         {
            boardIsConnected_ = value;
            if (true == value) BoardStatus = "Connected";
            else BoardStatus = "Not Connected";
            RaisePropertyChanged("BoardIsConnected");
         }
      }

      private String boardStatus_;
      public String BoardStatus
      {
         get { return boardStatus_; }
         set
         {
            boardStatus_ = value;
            RaisePropertyChanged("BoardStatus");
         }
      }


      public event PropertyChangedEventHandler PropertyChanged;
      public void RaisePropertyChanged(String prop)
      {
         if (null != PropertyChanged)
         {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
         }
      }
   }

   public class PointF_Replica
   {
      public Single X { get; set; }
      public Single Y { get; set; }
   }

}
