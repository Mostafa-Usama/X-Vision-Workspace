   M S B A M L   `   `   `  ����  Center_Maneger#    Center_Maneger.View.User_Note   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � 
ResizeMode$  NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	      win#��[  5   	    � Title$ ملاحظات��5   	   $	��300��6   $	��400��6&    � Icon$ /img/note.ico��5	   	   .��5
      � $	��15 q�6    � RowDefinitions 5   
   �� 5      $	��.2* �6   �� 5      $
��.55* �6   �� 5      $	��.2* �6   .+�5   
   �� .+�5      ��  �� Padding$ 3q�6     hoverbtn#��[ 6!   $	��150��6I     	Whitetext#��[ 6U    �� CornerRadius$ 7��6   $��5q�6�   ._�5      �� $��ملاحظات��6   $��18(�6+   $
��Bold"�69     text#��[ 6K   $��Center=�6n   5      5      �� 5   
   -   5        $��NoteTextBox��5   {   $��RightToLeft=�6    �� TextWrapping$
 Wrap=�6.    �� AcceptsReturn . "	 �� VerticalScrollBarVisibility$
	 Auto=�6W   $��1��6�   $
��auto��6�   $
��auto��6�   $�� 2q�6�   $��18(�6�   $
��Bold"�6�     RoundedTextBox#��[ 6�   �� 5   
   -   5        $��2��5        
Roundedbtn#��[ 6   $
��Bold"�6B   
������ $��35��6   $��Center=�6�   $��5q�6�   .��5      �� 
 �� Orientation$
 
Horizontal=�6   $��RightToLeft=�62   .+�5      �� $��حفظ��6     TextBlockStyle#��[ 6'   $��Center=�6O   �� 5      $��/img/save.png��6   5      5      5      5      