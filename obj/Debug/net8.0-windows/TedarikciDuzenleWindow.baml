   M S B A M L   `   `   `  ����B  >EggSoft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:EggSoftEggSoft  5   	   $    EggSoft.TedarikciDuzenleWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:EggSoft     5   	     � Title$/  )EggSoft - Tedarikçi Bilgilerini Düzenle��5   	   $	��350��69   $	��400��6F    � WindowStartupLocation$ CenterScreen=�5	   	   .��5
      � $��15q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $	��ID:��6   $��0��6   $��0��6,   $��5q�6<   �� 5   
   -   5         ��Name$ TxtId��5      $��0��6#   $��1��60   $��5q�6@   $
��Bold"�6K   �� 5   
   $	��Ad:��6   $��1��6   $��0��6,   $��5q�6<   �� 5   
   -   5         ��Name$ TxtAd��5      $��1��6!   $��1��6.   $��5q�6>   �� 5   
   $��Şehir:��6   $��2��6"   $��0��6/   $��5q�6?   �� 5   
   -   5        $ TxtSehir��5      $��2��6$   $��1��61   $��5q�6A   �� 5!   
   $��Telefon:��6   $��3��6$   $��0��61   $��5q�6A   �� 5"   
   -   5        $ 
TxtTelefon��5"      $��3��6&   $��1��63   $��5q�6C   �� 5$   
   -   5        $��Kaydet��5$      $��4��5%      $��1��6   $��5q�5&      $��Right=�6   $	��100��5'      6   5)      5*      