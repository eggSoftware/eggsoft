   M S B A M L   `   `   `  ����B  >EggSoft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:EggSoftEggSoft  5   	       EggSoft.YumurtaAlimWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:EggSoft     5   	     � Title$'  !EggSoft - Yumurta Alım Yönetimi��5   	   $	��700��61   $
��1000��6>    � WindowStartupLocation$ CenterScreen=�6K   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   
�������$��1q�66    �� CornerRadius$ 5��6J    �� Padding$ 10q�6[   $��0,0,0,10q�6h   ._�5      �� .+�5      �� $.��(Yumurta Alım Verilerini Dosyadan Yükle��6   $��16(�6J   $
��Bold"�6X   $��0,0,0,10q�6j   �� 5      $|��vLütfen Excel dosyanızı 'CSV (Virgülle Ayrılmış)' veya 'Metin (Sekmeyle Ayrılmış)' olarak kaydedip yükleyin.��6   $
��Wrap=�5      
�������$��0,0,0,5q�6>   � 5       � ColumnDefinitions 5      �� 5      $��* �6+   �� 5      $
��Auto �6+   .+�5      �� -   5         ��Name$ TxtFilePath��5      $��0��63    �� 
IsReadOnly$
 True��6C   $��0,0,5,0q�6U    � ToolTip$0 *Yüklenecek dosya yolu burada görünecek.��5      �� 5      -   5        $��
Dosya Seç��5      $��1��61   $	��100��6A   6M   5      5       5!      �� 5#   
   $��1��6   	 �� Orientation$	 
Horizontal=�6"   $��Right=�6;   $��0,0,0,10q�6W   .+�5$      �� -   5        $��Verileri Yükle / Kaydet��5$      $	��180��67   $��35��6C   $��5q�6O   6Z   �� 5%      -   5        $��Temizle Listeyi��5%      $	��120��6/   $��35��6;   $��5q�6G   6R   5&      &   System.Windows.Controls.DataGrid  5(   
   -   5        
  Name$
 YumurtaAlimDataGrid��5(      $��2��5)      $��0,0,0,10q�6       AutoGenerateColumns$ False��5*         
IsReadOnly$
 True��5+         SelectionMode$ Single=�5,         Columns 5-      0  *System.Windows.Controls.DataGridTextColumn  5.      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5/      $ Tarih��6%   $ * 6o    64   �� *Tarih+ �� StringFormat$ dd.MM.yyyy HH:mm��6o     50      $ Net KG��6%   $
 Auto 6b    65   �� *NetKg+$ N2��6b     51      $ Yumurta Gramajı��6%   $ * 6t    6>   �� *YumurtaGramaji+$ N2��6t     52      $
 Adet��6%   $
 Auto 6T    63   �� *YumurtaAdedi+6T     53      $ 	Etiket No��6%   $ * 6U    68   �� *
EtiketNo+6U     54      $ 	Cari Adı��6%   $ 2* 6R    67   �� *CariAd+6R     55      $ 	Stok Adı��6%   $ 2* 6R    67   �� *StokAd+6R   57      �� 59   
   -   5         ��Name$ StatusTextBlock��59      $��3��6-   $��5q�6:   
�������$/��)Uygulama mesajları burada gösterilecek.��6W   5:      5;      