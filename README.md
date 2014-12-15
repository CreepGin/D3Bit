The D3Bit project has long being discontinued. Source code here will be used for reference purposes.

---

D3Bit started during the first few months of the Diablo 3 lauch. it was a tooltip scanner for Diablo 3. One could use it to parse item stats, upload cropped tooltips, batch process screenshots, etc...


### Project Content

The project consists of 2 parts. One is a Dll library that exposes some of the image processing and Tesseract functions. The other part is the GUI.

Static classes Screenshot, Tesseract, and ImageUtil expose useful functions that you can call.


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

