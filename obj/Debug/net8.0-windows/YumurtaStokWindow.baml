   M S B A M L   `   `   `  ����B  >EggSoft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:EggSoftEggSoft  5   	       EggSoft.YumurtaStokWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:EggSoft     5   	     � Title$#  EggSoft - Yumurta Stok Durumu��5   	   $	��750��6/   $	��800��6<    � WindowStartupLocation$ CenterScreen=�6H   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   
�������$��1q�66    �� CornerRadius$ 5��6J    �� Padding$ 10q�6[   $��0,0,0,10q�6h   ._�5      �� .+�5      �� $��Mevcut Yumurta Stokları��6   $��20(�6;   $
��Bold"�6I   $��0,0,0,10q�6[   � 5      $	��0,5q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6+   �� 5      $��* �6+   �� 5      $
��Auto �6+   �� 5      $��* �6+   �� 5      $
��Auto �6+   �� 5      $��* �6+    5      �� 5      $
��Auto �6(   �� 5       $
��Auto �6(   �� 5!      $
��Auto �6(   .+�5$      �� $��Başlangıç Tarihi:��6    $��0��69   $��0��6F   $��5q�6V   (  "System.Windows.Controls.DatePicker  5%      -   5          Name$ DpBaslangicTarihi��5%   !   $��0��6<   $��1��6I   $��5q�6Y   �� 5'      $��Bitiş Tarihi:��6    $��0��65   $��2��6B   $��5q�6R     5(      -   5        $ DpBitisTarihi��5(   !   $��0��68   $��3��6E   $��5q�6U   �� 5*      $��Yumurta Tipi/Sınıfı:��6    $��1��6<   $��0��6I   $��5q�6Y   �� 5+      -   5         ��Name$ CmbYumurtaTipSinif��5+      $��1��6;   $��1��6H   $��5q�6X   �� 5-      $��Tedarikçi:��6    $��1��62   $��2��6?   $��5q�6O   �� 5.      -   5        $ CmbTedarikci��5.      $��1��65   $��3��6B   $��5q�6R    �� DisplayMemberPath$ Ad��5/      	 �� SelectedValuePath$	 Id��66   �� 50      -   5        $��Tedarikçileri Yenile��50      $��1��6<   $��4��6I   $	��130��6Y   $��30��6e   $��5,0,0,0q�6q   51      �� 52      $��Hafta Numarası:��6    $��2��67   $��0��6D   $��5q�6T   �� 53      -   5        
 ��Name$
 
TxtHaftaNo��53      $��2��62   $��1��6?   $��5q�6O    � ToolTip$$ Yılın Hafta Numarası (1-53)��6Z   �� 55      $��Yıl:��6    $��2��6,   $��2��69   $��5q�6I   �� 56      -   5        $
 TxtYil��56      $��2��6.   $��3��6;   $��5q�6K   $ Örnek: 2024��6V   57      58      59      �� 5;   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,0,0,10q�6W   .+�5<      �� -   5        $��Filtrele��5<      $	��100��6(   $��30��64   $��5q�6@   6K   �� 5=      -	   5        $��Temizle Filtreler��5=      $	��120��61   $��30��6=   $��5q�6I   6T   �� 5>      -
   5        $��Stokları Yenile��5>      $	��120��6/   $��30��6;   $��5q�6G   6R   5?      &   System.Windows.Controls.DataGrid  5A   
   -   5          Name$ YumurtaStokDataGrid��5A      $��2��5B      $��0,0,0,10q�6       AutoGenerateColumns$ False��5C         
IsReadOnly$
 True��5D         SelectionMode$ Single=�5E         Columns 5F      0  *System.Windows.Controls.DataGridTextColumn  5G      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Stok Adı��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6S   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *	StokAdi+6S     5H      $ Mevcut Adet��6%   $ * 6l    6:   �� *
MevcutAdet+ �� StringFormat$ N0��6l     5I      $ 	Mevcut KG��6%   $ * 6h    68   �� *
MevcutKg+$ N2��6h   5K      �� 5M   
   -   5         ��Name$ StatusTextBlock��5M      $��3��6-   $��5q�6:   
�������$/��)Uygulama mesajları burada gösterilecek.��6W   5N      5O      