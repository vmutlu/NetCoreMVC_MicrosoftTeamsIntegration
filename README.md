# Microsoft Teams Intagration 

Bu projede Microsoft Graph API'sini kullanarak .Net Core MVC projemize Microsoft Teams'ı entegre ediyoruz. Öncelikle Microsoft(https://developer.microsoft.com) sitesinden geliştirici hesabı oluşturup daha sonra oluşturdugumuz hesapla azure portalında uygulama kayıt ediyoruz. 

## :pushpin:Azure Uygulama Kayıt Ekranı
![2](https://user-images.githubusercontent.com/50150182/129455745-30807336-3c25-4c60-8ba0-72c1d745c446.JPG)

## :pushpin:Oluşturulan Uygulama

### Yeni Kayıt butonu ile uygulamamızı oluşturalım.

![3](https://user-images.githubusercontent.com/50150182/129455859-0d08e96b-d753-4fa6-bd15-03aa0d7b8cc0.JPG)

## :pushpin:Oluşturulan Uygulama Bilgileri
Buradaki uygulama ile olan alanlarımızı(ClientId, ClientSecret, ReturnUrl vb.) alarak projemizdeki appsetting.json dosyası içerisine yazıyoruz.

![4](https://user-images.githubusercontent.com/50150182/129455907-ab730de6-b5c3-4804-8dd3-5d105f3e3077.JPG)

### Oluşturduğumuz Uygulamaya Permission Değerlerini Tanımlama.
![1](https://user-images.githubusercontent.com/50150182/129455998-01bcdb3e-42e7-4a7c-8ff5-a2575f03d4e9.JPG)

* * *
### Projemizi Çalıştırıp Meeting Kayıt Ekranına Gelelim
Bu alanda Microsoft Teams üzerinden oluşturulacak meeting bilgilerini girip create butonuna tıklıyoruz. Yanlış tarih ve saate alınan meetingleriniz olduğunda endişelenmeyin çünkü oluşturulan meetingleri istediğimiz gibi silebilir ve güncelleyebiliriz :smiley:

&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ![5](https://user-images.githubusercontent.com/50150182/129455977-30589084-defd-432e-99f2-4ee212058569.JPG)

ve görüldüğü üzere artık meetingimiz oluşturulmuştur. Toplantıya katıl butonu ile toplantıya katılabiliriz.

![6](https://user-images.githubusercontent.com/50150182/129456218-707985d2-e813-4ac8-86f8-e585baf3c276.JPG)

### Oluşturduğumuz Meetingimizin Outlook takvimimizde görülmesi

![7](https://user-images.githubusercontent.com/50150182/129458476-9dd7d196-bfda-44c9-a2b3-9d5f1790deca.JPG)

### Oluşturduğumuz Meetingimizin Microsoft Teams takvimimizde görülmesi

![8](https://user-images.githubusercontent.com/50150182/129458504-9f93c8ea-a7be-4d40-ab6f-dd46167034b8.JPG)

:fire: Evet gördüğünüz üzere Microsoft Graph API'yi kullanarak projemize Microsoft Teams'ı entegre ettik. Graph API'nin birçok özelliği ve kullanımı hakkında "https://docs.microsoft.com/en-us/graph/overview" Microsoftun dökümanını inceleyerek geliştirmeler yapabilirsiniz. İlgilenlere faydalı olması dileğiyle. :wave:








