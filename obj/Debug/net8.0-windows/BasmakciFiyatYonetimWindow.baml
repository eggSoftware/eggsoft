   M S B A M L   `   `   `  ����B  >EggSoft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:EggSoftEggSoft  5   	   (    "EggSoft.BasmakciFiyatYonetimWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:EggSoft     5   	     � Title$0  *EggSoft - Başmakçı Fiyatları Yönetimi��5   	   $	��500��67   $	��600��6D    � WindowStartupLocation$ CenterScreen=�6P   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $
��Left=�6;   $��0,0,0,10q�6V   .+�5      �� -   5        $!��Başmakçı Fiyatını Çek��5      $	��180��5      $��30��6!   $��0,0,10,0q�5      5      �� 5      -   5         ��Name$ TxtSonCekimDurumu��5      $��Center=�63   $��5,0,0,0q�6N   $��Son çekim: Yok��5      $��12(�6.   
�������6<   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ BasmakciFiyatDataGrid��5      $��1��5      $��0,0,0,10q�6       AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 ID��6%   
   Width5  /System.Windows.Controls.DataGridLengthConverter$

 Auto 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5      $	 Tarih��6%   $
 * 6i    64   �� *Tarih+ �� StringFormat$ 
dd.MM.yyyy��6i     5       $	 Duble Fiyatı (TL)��6%   $
 * 6z    6@   �� *DubleYumurtaFiyati+$ C2��6z     5!      $	 Hafta No��6%   $

 Auto 6Y    67   �� *HaftaNumarasi+6Y   5#      �� 5%   
   -   5        $ StatusTextBlock��5%      $��2��6-   $��5q�6:   
�������$/��)Uygulama mesajları burada gösterilecek.��6W   5&      5'      