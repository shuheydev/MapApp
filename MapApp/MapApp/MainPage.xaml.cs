using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace MapApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //Mapの描画が終わるのを待っている?
            Task.Delay(2000);
            UpdateMap();
        }


        ObservableCollection<Place> placesList = new ObservableCollection<Place>();
        //ObservableCollection<Place2> placesList2 = new ObservableCollection<Place2>();


        private void UpdateMap()
        {
            try
            {
                //Embedded resourceのPlaces.jsonを読み込む.文字列として
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("MapApp.Places.json");
                string text = string.Empty;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                //文字列のJsonをPlacesオブジェクトに変換する
                var resultObject = JsonConvert.DeserializeObject<Places>(text);

                //場所の情報PlaceをListに追加する.
                foreach (var place in resultObject.results)
                {
                    placesList.Add(new Place
                    {
                        PlaceName = place.name,
                        Address = place.vicinity,
                        Location = place.geometry.location,
                        Position = new Position(place.geometry.location.lat, place.geometry.location.lng),
                        //Icon = place.icon,
                        //Distance = $"{GetDistance(lat1, lon1, place.geometry.location.lat, place.geometry.location.lng, DistanceUnit.Kiliometers).ToString("N2")}km",
                        //OpenNow = GetOpenHours(place?.opening_hours?.open_now)
                    });

                    var pin = new Xamarin.Forms.GoogleMaps.Pin()
                    {
                        Type = Xamarin.Forms.GoogleMaps.PinType.Place,
                        Label = place.name,
                        Address = place.vicinity,
                        Position = new Xamarin.Forms.GoogleMaps.Position(place.geometry.location.lat, place.geometry.location.lng),
                        Rotation = 33.3f,
                        Tag = "",
                        //IsVisible = switchIsVisibleTokyo.IsToggled
                    };
                    MyMap2.Pins.Add(pin);
                    //MyMap2.Add(new Place2
                    //{
                    //    PlaceName = place.name,
                    //    Address = place.vicinity,
                    //    Location = place.geometry.location,
                    //    Position = new Xamarin.Forms.GoogleMaps.Position(place.geometry.location.lat, place.geometry.location.lng),
                    //    //Icon = place.icon,
                    //    //Distance = $"{GetDistance(lat1, lon1, place.geometry.location.lat, place.geometry.location.lng, DistanceUnit.Kiliometers).ToString("N2")}km",
                    //    //OpenNow = GetOpenHours(place?.opening_hours?.open_now)
                    //});
                }

                MyMap.ItemsSource = placesList;
                //MyMap2.Pins.Add() = placesList2;
                //PlacesListView.ItemsSource = placesList;
                //var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(47.6370891183, -122.123736172), Distance.FromKilometers(100)));
                MyMap2.MoveToRegion(Xamarin.Forms.GoogleMaps.MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(47.6370891183, -122.123736172), Xamarin.Forms.GoogleMaps.Distance.FromKilometers(100)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void MyMap2_MapLongClicked(object sender, Xamarin.Forms.GoogleMaps.MapLongClickedEventArgs e)
        {
            //var pin = new Pin
            //{
            //    Position = new Position(e.Point.Latitude, e.Point.Longitude)
            //};

            var pin = new Xamarin.Forms.GoogleMaps.Pin()
            {
                Type = Xamarin.Forms.GoogleMaps.PinType.Place,
                Label = "New Place",
                Address = "Hello world",
                Position = new Xamarin.Forms.GoogleMaps.Position(e.Point.Latitude, e.Point.Longitude),
                Rotation = -33.3f,
                Tag = "",
                //IsVisible = switchIsVisibleTokyo.IsToggled
            };
            //var place = new Place2
            //{
            //    PlaceName = "New Place",
            //    Address = "hello",
            //    Location = new Location { lat = (float)e.Point.Latitude, lng = (float)e.Point.Longitude },
            //    Position = new Xamarin.Forms.GoogleMaps.Position(e.Point.Latitude, e.Point.Longitude),
            //    //Icon = place.icon,
            //    //Distance = $"{GetDistance(lat1, lon1, place.geometry.location.lat, place.geometry.location.lng, DistanceUnit.Kiliometers).ToString("N2")}km",
            //    //OpenNow = GetOpenHours(place?.opening_hours?.open_now)
            //};

            pin.IsDraggable = true;
            
            MyMap2.Pins.Add(pin);
        }

        private void MyMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            var pin = new Pin
            {
                Position = e.Position
            };
            var place = new Place
            {
                PlaceName = "New Place",
                Address = "hello",
                Location = new Location { lat = (float)e.Position.Latitude, lng = (float)e.Position.Longitude },
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
                //Icon = place.icon,
                //Distance = $"{GetDistance(lat1, lon1, place.geometry.location.lat, place.geometry.location.lng, DistanceUnit.Kiliometers).ToString("N2")}km",
                //OpenNow = GetOpenHours(place?.opening_hours?.open_now)
            };

            placesList.Add(place);
        }
    }
}
