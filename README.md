# Uno Templated Dialog Issues
Replicates issues with templated content dialog controls

Raised as issue #4639
Also raised as issue #3751 on https://github.com/Microsoft/microsoft-ui-xaml
On UWP the following exception is raised

System.AccessViolationException
  HResult=0x80004003
  Message=Attempted to read or write protected memory. This is often an indication that other memory is corrupt.
  Source=<Cannot evaluate the exception source>
  StackTrace:
<Cannot evaluate the exception stack trace>

Raised as issue #4641
On Android, iOS and WASM The content dialog is displayed before ShowAsync is called
On Android, iOS and WASM The content dialog is displayed in the wrong location after ShowAsync is called

Raised as issue #4640
On Android the following exception is raised when the 'Show Progress' button is pressed

Java.Lang.IllegalStateException
  Message=The specified child already has a parent. You must call removeView() on the child's parent first.


The positioning issue has been raised separately as #4662

A manual adjustment of positions added showing a workaround for the positioning issue