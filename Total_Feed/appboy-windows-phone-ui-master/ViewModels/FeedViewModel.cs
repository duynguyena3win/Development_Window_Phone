using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using AppboyPlatform.PCL.Models.Incoming.Cards;
using AppboyPlatform.PCL.Results;
using AppboyPlatform.Phone;
using AppboyUI.Phone.Annotations;

namespace AppboyUI.Phone.ViewModels {
  public class FeedViewModel : INotifyPropertyChanged {
    private bool _networkUnavailable;
    private bool _refreshingFeed;
    public event PropertyChangedEventHandler PropertyChanged;
    public ObservableCollection<BaseCard> Cards { get; set; }

    public FeedViewModel() {
      Cards = new ObservableCollection<BaseCard>();
    }

    public bool EmptyFeed {
      get {
        return NetworkUnavailable == false && RefreshingFeed == false && Cards.Count == 0;
      }
    }

    public bool NetworkUnavailable {
      get {
        return _networkUnavailable;
      }
      private set {
        if (value != _networkUnavailable) {
          _networkUnavailable = value;
          OnPropertyChanged("EmptyFeed");
        }
      }
    }

    public bool RefreshingFeed {
      get {
        return _refreshingFeed;
      }
      private set {
        if (value != _refreshingFeed) {
          _refreshingFeed = value;
          OnPropertyChanged("EmptyFeed");
        }
      }
    }

    public void GetFeed() {
      RefreshingFeed = true;
      Action<Task<IResult>> bindCards = (continuation) => {
        NetworkUnavailable = continuation.Result.ErrorType == ErrorType.NetworkUnavailable;
        Deployment.Current.Dispatcher.BeginInvoke(() => {
          Cards.Clear();
          foreach (BaseCard card in continuation.Result.Cards ?? Enumerable.Empty<BaseCard>()) {
            Cards.Add(card);
          }
        });
        RefreshingFeed = false;
      };
      Appboy.SharedInstance.RequestFeed().ContinueWith(bindCards);
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) {
        Deployment.Current.Dispatcher.BeginInvoke(() => {
          handler(this, new PropertyChangedEventArgs(propertyName));
        });
      }
    }
  }
}
