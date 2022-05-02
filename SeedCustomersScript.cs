Public Sub GenerateCustomersSeed()
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
    Set wksht = ThisWorkbook.Worksheets("Customers")
    
    'create a variable for the used range of the sheet
    Dim rng As Range
    
    'set range equal to used range
    Set rng = wksht.UsedRange
    
    'create an output file for all of this stuff
    Set fs = CreateObject("Scripting.FileSystemObject")
    
    'get file name to save as
    Filename = Application.GetSaveAsFilename("SeedCustomers", "C# Classes (*.cs), *.cs", Title:="Select a file name")
    
    'create output file
    Set outputfile = fs.CreateTextFile(Filename, True)
    For i = 2 To 32
            
            'parse in the values from the spreadsheet - Chr(34) prints a double quote (") to the file
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser = new AppUser();")
            outputfile.WriteLine (vbTab & vbTab & vbTab)
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.UserName = " & Chr(34) & wksht.UsedRange(i, 16) & Chr(34) & ";")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.Email = " & Chr(34) & wksht.UsedRange(i, 16) & Chr(34) & ";")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.PhoneNumber = " & Chr(34) & wksht.UsedRange(i, 15) & Chr(34) & ";")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.FirstName = " & Chr(34) & wksht.UsedRange(i, 4) & Chr(34) & ";")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.MiddleInitial = " & Chr(34) & wksht.UsedRange(i, 5) & Chr(34) & ";")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.LastName = " & Chr(34) & wksht.UsedRange(i, 3) & Chr(34) & ";")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.Birthdate = new DateTime(" & wksht.UsedRange(i, 7) & "," & wksht.UsedRange(i, 8) & "," & wksht.UsedRange(i, 9) & ");")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.Address = " & Chr(34) & wksht.UsedRange(i, 10) & " " & wksht.UsedRange(i, 11) & ", " & wksht.UsedRange(i, 12) & wksht.UsedRange(i, 13) & Chr(34) & ";")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser.PopcornPoints = " & wksht.UsedRange(i, 17) & ";")
            outputfile.WriteLine (vbTab & vbTab & vbTab)
            outputfile.WriteLine (vbTab & vbTab & vbTab & "result = new IdentityResult();")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "result = await _userManager.CreateAsync(newUser," & Chr(34) & wksht.UsedRange(i, 2) & Chr(34) & ");")
            outputfile.WriteLine (vbTab & vbTab & vbTab)
            outputfile.WriteLine (vbTab & vbTab & vbTab & "_context.SaveChanges();")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "newUser = _context.Users.FirstOrDefault(u => u.UserName == " & Chr(34) & wksht.UsedRange(i, 16) & Chr(34) & ");")
            outputfile.WriteLine (vbTab & vbTab & vbTab)
            outputfile.WriteLine (vbTab & vbTab & vbTab & "if (await _userManager.IsInRoleAsync(newUser, " & Chr(34) & "Customer" & Chr(34) & ") == false)")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "{")
            outputfile.WriteLine (vbTab & vbTab & vbTab & vbTab & "await _userManager.AddToRoleAsync(newUser, " & Chr(34) & "Customer" & Chr(34) & ");")
            outputfile.WriteLine (vbTab & vbTab & vbTab & "}")
            outputfile.WriteLine (vbTab & vbTab & vbTab)
            outputfile.WriteLine (vbTab & vbTab & vbTab & "_context.SaveChanges();")
            outputfile.WriteLine (vbTab & vbTab & vbTab)
            
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
