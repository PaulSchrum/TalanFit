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
         Gem = new BalanceGem();
         Gem.GemOpacity = 1.0;
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
            if (this.Gem != null)
               this.Gem.X = value;
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
            if (this.Gem != null)
               this.Gem.Y = value;
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

      public BalanceGem Gem { get; set; }

   }

   // public class PointF_Replica is no longer needed

   public class BalanceGem : INotifyPropertyChanged
   {
      public BalanceGem()
      { 
         parentHeight = parentWidth = 4*96.0F;
         X = Y = 0.75F * 96.0F;
      }

      private Single gem_x_;
      public Single X
      {
         get { return gem_x_; }
         set
         {
            gem_x_ = computeActualGemParameter(this.parentWidth, value);
            RaisePropertyChanged("X");
         }
      }

      private Single gem_y_;
      public Single Y
      {
         get { return gem_y_; }
         set
         {
            gem_y_ = computeActualGemParameter(this.parentHeight, value);
            RaisePropertyChanged("Y");
         }
      }

      public Single computeActualGemParameter(Single AxisLength, Single value)
      {
         Single retVal = value * 10.0F;
         //retVal /= AxisLength;
         retVal += AxisLength / 2.0F;
         return retVal;
      }

      internal Single maxX { get; set; }
      internal Single maxY { get; set; }
      public Single parentHeight { get; set; }
      public Single parentWidth { get; set; }

      private Double gemOpacity_;
      public Double GemOpacity
      {
         get { return gemOpacity_; }
         set
         {
            gemOpacity_ = value;
            RaisePropertyChanged("GemOpacity");
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

}
