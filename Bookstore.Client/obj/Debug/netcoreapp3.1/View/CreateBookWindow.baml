   M S B A M L   `   `   `  ����K  GBookstore.Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Bookstore.Client.ViewBookstore.Client.View  5   	   ,    &Bookstore.Client.View.CreateBookWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:Bookstore.Client.View     5   	   k gDotNetProjects.Wpf.Extended.Toolkit, Version=4.6.97.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  Create A Book��5	   	   $	��250��6   $	��250��6,   .��5
      � $��10q�6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0��6   $��ISBN:��6-   �� 5   
   -   5        $��ISBNName��5   :   $��0��6   $��1��6   $��5q�6/   ��5      �� *newBook.ISBN+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   $��1��6   $��0��6   $��Title:��6-   �� 5   
   -   5        $��	TitleName��5   :   $��1��6   $��1��6   $��5q�6/   ��5      �� *newBook.Title+$ PropertyChanged=��� 5   
   $��2��6   $��0��6   $��Author:��6-   �� 5    
   -   5        $��
AuthorName��5    :   $��2��6   $��1��6   $��5q�6/   ��5!      �� *newBook.Author+$ PropertyChanged=��� 5#   
   $��3��6   $��0��6   $��Published in:��6-   &   Xceed.Wpf.Toolkit.DateTimePicker  5$   
   -   5        $��PublishDateName��5$   ;   $��3��6   $��1��6+   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.DateTime, System.Runtime, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]], System.Runtime, Version=4.2.2.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a]]   Value 6R   �� *newBook.PublishDate+�� 5&   
   -   5        $��ImageStateLabel��5&   -   $��4��6   $��0��6    J� Opacity$	 0.5Q�6D   $��No Image��6R   �� 5'   
   $��4��6   $��1��6   $��1��6.   $��Upload Cover Image��6B   $��Center=�5(      $��Center=�6.   $��2q�5)      $��5q�6   ��5*      �� *UploadImageCommand+�� 5,   
   $��5��6   $��0��6   $��1��6.   $��Create��6B   $��Center=�5-      $��Center=�6.   $��2q�5.      $��5q�6   ��5/      �� *CreateButtonCommand+�� 51   
   $��5��6   $��1��6   $��1��6.   $��Cancel��6B   $��Center=�52      $��Center=�6.   $��2q�53      $��5q�6   ��54      �� *CancelButtonCommand+55      56      