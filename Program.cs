using System.Drawing;

internal class Program
{
    private static void Main() {
        //move up 3 directorys from local
        string localDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;


        //create output directory if it doesn't exist
        string outputDir = localDir + @"\Output";
        if (!Directory.Exists(outputDir)) {
            Directory.CreateDirectory(outputDir);
        }

        //get all png and bmp files in InputA and InputB directories
        string[] inputAFiles = Directory.GetFiles(localDir + @"\InputA", "*.png");
        string[] inputBFiles = Directory.GetFiles(localDir + @"\InputB", "*.png");
        inputAFiles = inputAFiles.Concat(Directory.GetFiles(localDir + @"\InputA", "*.bmp")).ToArray();
        inputBFiles = inputBFiles.Concat(Directory.GetFiles(localDir + @"\InputB", "*.bmp")).ToArray();

        //when the name from InputA matches the name from InputB, compare the images
        foreach (string inputAFile in inputAFiles) {
            foreach (string inputBFile in inputBFiles) {
                if (Path.GetFileName(inputAFile) == Path.GetFileName(inputBFile)) {
                    Console.WriteLine("Comparing " + Path.GetFileName(inputAFile) + "...");
                    //create 2 new image files from the input files

                    Image A = Image.FromFile(inputAFile);

                    Image B = Image.FromFile(inputBFile);

                    Bitmap imageA = new(A);
                    Bitmap imageB = new(B);

                    //create 2 new image files to hold the comparison
                    Bitmap imageC = new(imageA.Width, imageA.Height);
                    Bitmap imageD = new(imageA.Width, imageA.Height);

                    //do a pixel by pixel comparison of the images and set pixels that are different from image A to image C and image B to image D
                    for (int x = 0; x < imageA.Width; x++) {
                        //write to console the progress of the comparison every 512 pixels as a presentage
                        if (x % 512 == 0) {
                            Console.WriteLine("\t" + (x / (float)imageA.Width * 100).ToString("0.00") + "%");
                        }

                        for (int y = 0; y < imageA.Height; y++) {
                            if (imageA.GetPixel(x, y) != imageB.GetPixel(x, y)) {
                                imageC.SetPixel(x, y, imageA.GetPixel(x, y));
                                imageD.SetPixel(x, y, imageB.GetPixel(x, y));
                            }
                        }
                    }
                    //save the comparison images to the output directory with the same name as the input images but with a postfix of _A and _B
                    imageC.Save(outputDir + @"\" + Path.GetFileNameWithoutExtension(inputAFile) + "_A.png");
                    imageD.Save(outputDir + @"\" + Path.GetFileNameWithoutExtension(inputBFile) + "_B.png");
                }
            }
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

}