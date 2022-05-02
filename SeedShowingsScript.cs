Public Sub GenerateShowingSeed()
    'Declare some variables for counters
    Dim i As Integer
    Dim intCount As Integer
    Dim strShowing As String
    Dim j As Integer
    
    'Start counter at 1
    intCount = 1
    
    'Create a variable to reference the worksheet
    Dim wksht As Worksheet
    
    'set the variable equal to the currently active sheet
    Set wksht = ThisWorkbook.Worksheets("Movie Schedule for December 4-1")
    
    'create a variable for the used range of the sheet
    Dim rng As Range
    
    'set range equal to used range
    Set rng = wksht.UsedRange
    
    'create an output file for all of this stuff
    Set fs = CreateObject("Scripting.FileSystemObject")
    
    'get file name to save as
    Filename = Application.GetSaveAsFilename("SeedShowings", "C# Classes (*.cs), *.cs", Title:="Select a file name")
    
    'create output file
    Set outputfile = fs.CreateTextFile(Filename, True)
    For j = 4 To 10
        For i = 3 To 8
                
                outputfile.WriteLine (vbTab & vbTab & "AllShowings.Add(new Showing")
                outputfile.WriteLine (vbTab & vbTab & "{")
                
                'parse in the values from the spreadsheet - Chr(34) prints a double quote (") to the file
                outputfile.WriteLine (vbTab & vbTab & vbTab & "StartDateTime = new DateTime(" & "2020" & "," & "12" & "," & j & "," & wksht.UsedRange(i, 2) & "," & wksht.UsedRange(i, 3) & ",0),")
                outputfile.WriteLine (vbTab & vbTab & vbTab & "EndDateTime = new DateTime(" & "2020" & "," & "12" & "," & j & "," & wksht.UsedRange(i, 5) & "," & wksht.UsedRange(i, 6) & ",0),")
                outputfile.WriteLine (vbTab & vbTab & vbTab & "Room = 1,")
                outputfile.WriteLine (vbTab & vbTab & vbTab & "SpecialEvent = false,")
                outputfile.WriteLine (vbTab & vbTab & vbTab & "Movie = db.Movies.FirstOrDefault(c => c.Title == " & Chr(34) & wksht.UsedRange(i, 7) & Chr(34) & "),")
                outputfile.WriteLine (vbTab & vbTab & "});")
            Next i
        Next j
    
    
    For j = 4 To 10
        For i = 3 To 9
                
                outputfile.WriteLine (vbTab & vbTab & "AllShowings.Add(new Showing")
                outputfile.WriteLine (vbTab & vbTab & "{")
                
                'parse in the values from the spreadsheet - Chr(34) prints a double quote (") to the file
                outputfile.WriteLine (vbTab & vbTab & vbTab & "StartDateTime = new DateTime(" & "2020" & "," & "12" & "," & j & "," & wksht.UsedRange(i, 11) & "," & wksht.UsedRange(i, 12) & ",0),")
                outputfile.WriteLine (vbTab & vbTab & vbTab & "EndDateTime = new DateTime(" & "2020" & "," & "12" & "," & j & "," & wksht.UsedRange(i, 14) & "," & wksht.UsedRange(i, 15) & ",0),")
                outputfile.WriteLine (vbTab & vbTab & vbTab & "Room = 2,")
                outputfile.WriteLine (vbTab & vbTab & vbTab & "SpecialEvent = false,")
                outputfile.WriteLine (vbTab & vbTab & vbTab & "Movie = db.Movies.FirstOrDefault(c => c.Title == " & Chr(34) & wksht.UsedRange(i, 16) & Chr(34) & "),")
                outputfile.WriteLine (vbTab & vbTab & "});")
            Next i
        Next j
'
'
'
    'close the file
    outputfile.Close
    
    'tell user we're done
    MsgBox ("File Created Successfully!")
'
End Sub




