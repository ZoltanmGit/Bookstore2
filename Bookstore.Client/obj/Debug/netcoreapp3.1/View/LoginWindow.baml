   M S B A M L   `   `   `  ����K  GBookstore.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Bookstore.Client.ViewBookstore.Client.View  5   	   '    !Bookstore.Client.View.LoginWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:Bookstore.Client.View     5   	     � Title$  Bookstore Login��5   	   $	��160��6!   $	��250��6.   .��5	      � $��10q�6    � ColumnDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6   $��Name:��6-   �� 5   
   -   5        $��txtName��5   :   $��0��6   $��1��6   $��5q�6/   ��5      �� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��1��6   $��0��6   $��	Password:��6-   G� 5   
   -   5        $��txtPassword��5   3   $��1��6   $��1��6#   $��5q�6F   �� 5   
   $��2��6   $��2��6   $��Login��62   $��Center=�5      $��Center=�6.   $��2q�5      $��5q�6    �� 	IsDefault 5      �� *
bIsLoading+{�5      �� *bIsLoginEnabled+$ PropertyChanged=���5      �� *LoginCommand+��5       ��  �� ElementName$ txtPassword��5!      5"      