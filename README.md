D3Bit is a tooltip scanner for Diablo 3. You can use it to parse item stats, upload cropped tooltips, batch process screenshots, etc...

[D3Bit.com](http://d3bit.com/) - [Demo Youtube Video](http://www.youtube.com/watch?v=-mVm4cAsURk)

### Project Content

The project consists of 2 parts. One is a Dll library that exposes some of the image processing and Tesseract functions. The other part is the GUI.

Static classes Screenshot, Tesseract, and ImageUtil all expose useful functions that you can call.


    var procs = Process.GetProcessesByName("Diablo III");
    Bitmap bitmap = Screenshot.GetSnapShot(procs[0]);
    var res = Screenshot.GetToolTip(bitmap);
    res.Save("z.png", ImageFormat.Png);
    Tooltip tooltip = new D3Bit.Tooltip(res);
    string name = tooltip.ParseItemName();
    string quality = "";
    string type = tooltip.ParseItemType(out quality);
    double dps = tooltip.ParseDPS();
    var affixes = tooltip.ParseAffixes();
    tooltip.Processed.Save("s.png", ImageFormat.Png);

Sorry for the lack of doc and comment right now. But I'll try to help anyone with any problem over at the [Support Forum](http://d3bit.com/discuss/support/).