Public Sub GenerateMovieSeed()
    'Declare some variables for counters
    Dim i As Integer
    Dim intCount As Integer
    Dim strMovie As String
    
    'Start counter at 1
    intCount = 1
    
    'Create a variable to reference the worksheet
    Dim wksht As Worksheet
    
    'set the variable equal to the currently active sheet
    Set wksht = ThisWorkbook.Worksheets("Movies")
    
    'create a variable for the used range of the sheet
    Dim rng As Range
    
    'set range equal to used range
    Set rng = wksht.UsedRange
    
    'create an output file for all of this stuff
    Set fs = CreateObject("Scripting.FileSystemObject")
    
    'get file name to save as
    Filename = Application.GetSaveAsFilename("SeedMovies", "C# Classes (*.cs), *.cs", Title:="Select a file name")
    
    'create output file
    Set outputfile = fs.CreateTextFile(Filename, True)
    For i = 3 To 101
            
            outputfile.WriteLine (vbTab & vbTab & "AllMovies.Add(new Movie")
            outputfile.WriteLine (vbTab & vbTab & "{")
            
            'parse in the values from the spreadsheet - Chr(34) prints a double quote (") to the file
            outputfile.WriteLine (vbTab & vbTab & vbTab & "MovieID = " & wksht.UsedRange(i, 1) & ",")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "Title = " & Chr(34) & wksht.UsedRange(i, 2) & Chr(34) & ",")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "Description = " & Chr(34) & wksht.UsedRange(i, 4) & Chr(34) & ",")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "ReleaseDate = new DateTime(" & wksht.UsedRange(i, 6) & "," & wksht.UsedRange(i, 7) & "," & wksht.UsedRange(i, 8) & "),")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "Revenue = " & wksht.UsedRange(i, 9) & ",")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "Runtime = " & wksht.UsedRange(i, 10) & ",")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "MPAA = " & "MPAA." & wksht.UsedRange(i, 12) & ",")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "Tagline = " & Chr(34) & wksht.UsedRange(i, 11) & Chr(34) & ",")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "Actors = " & Chr(34) & wksht.UsedRange(i, 13) & Chr(34) & ",")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "Genre = db.Genres.FirstOrDefault(c => c.GenreName == " & Chr(34) & wksht.UsedRange(i, 3) & Chr(34) & "),")
            outputfile.WriteLine (vbTab & vbTab & "});")
        Next i
'
'
'
    'close the file
    outputfile.Close
    
    'tell user we're done
    MsgBox ("File Created Successfully!")
'
End Sub



