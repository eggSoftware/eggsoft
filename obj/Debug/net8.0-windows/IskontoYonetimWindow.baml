   M S B A M L   `   `   `  ����B  >EggSoft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:EggSoftEggSoft  5   	   "    EggSoft.IskontoYonetimWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:EggSoft     5   	     � Title$,  &EggSoft - İskonto Oranları Yönetimi��5   	   $	��650��65   $	��800��6B    � WindowStartupLocation$ CenterScreen=�6N   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   .+�5   
   �� $��0��6   
�������$��1q�66    �� CornerRadius$ 5��6J    �� Padding$ 10q�6[   $��0,0,0,10q�6h   ._�5      �� .+�5      �� $2��,Yeni İskonto Oranı Ekle / Mevcutu Düzenle��6   $��16(�6M   $
��Bold"�6[   $��0,0,0,10q�6m   � 5       5      �� 5      $
��Auto �6+   �� 5      $��* �6+   �� 5      $
��Auto �6+   �� 5      $��* �6+   �� 5      $
��Auto �6+    5       �� 5!      $
��Auto �6(   �� 5"      $
��Auto �6(   �� 5#      $
��Auto �6(   .+�5&      �� $��Tedarikçi:��6    $��0��62   $��0��6?   $��5q�6O   �� 5'      -   5         ��Name$ CmbTedarikci��5'      $��0��65   $��1��6B   $��5q�6R    �� DisplayMemberPath$ Ad��5(       �� SelectedValuePath$ Id��66   �� 5)      -   5        $��Tedarikçileri Yenile��5)      $��0��6<   $��2��6I   $	��130��6Y   $��30��6e   $��5,0,0,0q�6q   5*      �� 5+      $��Yumurta Tipi:��6    $��1��65   $��0��6B   $��5q�6R   �� 5,      -   5        $ CmbYumurtaTipi��5,      $��1��67   $��1��6D   $��5q�6T   �� 5.      $��Hafta Numarası:��6    $��2��67   $��0��6D   $��5q�6T   �� 5/      -   5        	 ��Name$	 TxtHaftaNumarasi��5/      $��2��68   $��1��6E   $��5q�6U   
 � ToolTip$*
 $Yılın Hafta Numarası (örn. 1-53)��50      �� 52      $��	Oran (%):��6    $��2��61   $��2��6>   $��5q�6N   �� 53      -   5        $	 TxtOran��53      $��2��6/   $��3��6<   $��5q�6L   54      55      56      �� 58   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,0,0,10q�6W   .+�59      �� -   5        $��	Yeni Ekle��59      $	��100��6)   $��5q�65   6@   �� 5:      -   5        $��	Güncelle��5:      $	��100��6(   $��5q�64   6?   �� 5;      -   5        $	��Sil��5;      $	��100��6#   $��5q�6/   6:   �� 5<      -	   5        $��Temizle��5<      $	��100��6'   $��5q�63   6>   5=      &   System.Windows.Controls.DataGrid  5?   
   -
   5          Name$ IskontoDataGrid��5?      $��2��5@      $��0,0,0,10q�6       AutoGenerateColumns$ False��5A         
IsReadOnly$
 True��5B         Columns 5D      0  *System.Windows.Controls.DataGridTextColumn  5E      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5F      $ Tedarikçi Adı��6%   $ * 6\    6<   �� *TedarikciAd+6\     5G      $ Yumurta Tipi��6%   $
 Auto 6[    6;   �� *YumurtaTipi+6[     5H      $ Hafta No��6%   $
 Auto 6Y    67   �� *HaftaNumarasi+6Y     5I      $
 Oran��6%   $
 Auto 6^    63   �� *Oran+ �� StringFormat$ P��6^   5K      �� 5M   
   -   5         ��Name$ StatusTextBlock��5M      $��3��6-   $��5q�6:   
�������$,��&Durum mesajları burada gösterilecek.��6W   5N      5O      