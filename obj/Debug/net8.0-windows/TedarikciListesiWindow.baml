   M S B A M L   `   `   `  ����B  >EggSoft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:EggSoftEggSoft  5   	   $    EggSoft.TedarikciListesiWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:EggSoft     5   	     � Title$"  EggSoft - Tedarikçi Listesi��5   	   $	��600��6-   $	��800��6:    � WindowStartupLocation$ CenterScreen=�6F   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   o� $��0��6   $��1��6   
�������.@�5      m� $��_Dosya��6   .@�5      m� -   5        $��
_Çıkış��5      6+   5      m� 5      $��	_Düzenle��6   .@�5      m� -   5        $��_Yeni Tedarikçi Ekle��5      69   m� 5      -   5        $��Tedarikçiyi _Düzenle��5      69   m� 5      -   5        $��Tedarikçiyi _Sil��5      65   5      5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ TedarikciDataGrid��5         AutoGenerateColumns$
 True��5         
IsReadOnly$
 True��5       $��10q�5!      $��1��5"      $��1��5#      5$      �� 5&   
   $��2��6   $��1��6"   $��10q�66   .+�5'      �� $$��Seçilen Tedarikçi Detayları��6   $��18(�6;   $
��Bold"�6I   $��0,0,0,10q�6[   � 5)      $	��0,5q�6    5*      �� 5+      $
��Auto �6'   �� 5,      $��* �6'   .+�5.      �� $	��ID:��6   $
��Bold"�6'   $��0��69   $��0,0,5,0q�6I   �� 5/      -   5         ��Name$ 
TxtDetayId��5/      $��1��60   50      � 52      $	��0,5q�6    53      �� 54      $
��Auto �6'   �� 55      $��* �6'   .+�57      �� $	��Ad:��6   $
��Bold"�6'   $��0��69   $��0,0,5,0q�6I   �� 58      -   5        $ 
TxtDetayAd��58      $��1��60   59      � 5;      $	��0,5q�6    5<      �� 5=      $
��Auto �6'   �� 5>      $��* �6'   .+�5@      �� $��Şehir:��6   $
��Bold"�6*   $��0��6<   $��0,0,5,0q�6L   �� 5A      -   5        $ TxtDetaySehir��5A      $��1��63   5B      � 5D      $	��0,5q�6    5E      �� 5F      $
��Auto �6'   �� 5G      $��* �6'   .+�5I      �� $��Telefon:��6   $
��Bold"�6,   $��0��6>   $��0,0,5,0q�6N   �� 5J      -	   5        $ TxtDetayTelefon��5J      $��1��65   5K      �� 5M       �� Orientation$ 
Horizontal=�6   $��Right=�62   $��0,15,0,0q�6N   .+�5N      �� -
   5        $��Yeni Tedarikçi Ekle��5N      $	��150��67   $��30��6C   $��5q�6O   6Z   �� 5O      -   5        $��Seçileni Düzenle��5O      $	��150��64   $��30��6@   $��5q�6L   6W   �� 5P      -   5        $��Seçileni Sil��5P      $	��150��60   $��30��6<   $��5q�6H   6S   5Q      5R      5S      5T      