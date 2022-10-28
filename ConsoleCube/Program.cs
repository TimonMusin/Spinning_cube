using ConsoleCube;
using System.Net.Http.Headers;
using System.Xml.Serialization;


internal class Program
{
    private static void Main(string[] args)
    {
        
        double A = 0;
        double B = 0;
        double C = 0;

        double cubeWidth = 20;
        int width = 160, height = 44;
        double[] zBuffer = new double[160 * 44];
        double[] buffer = new double[160 * 44];
        int backgroundASCIICode = '-';
        int distanceFromCam = 150;
        double horizontalOffset;
        double K1 = 70;

        double incrementSpeed = 1.3;

        double x, y, z;
        double ooz;
        int xp, yp;
        int idx;
        
        double cos(double v)
        {
            return Math.Cos(v);
        }
        double sin(double v)
        {
            return Math.Sin(v);
        }


        double calculateX(double i, double j, double k)
        {
            return j * sin(A) * sin(B) * cos(C) - k * cos(A) * sin(B) * cos(C) +
                   j * cos(A) * sin(C) + k * sin(A) * sin(C) + i * cos(B) * cos(C);
        }

        double calculateY(double i, double j, double k)
        {
            return j * cos(A) * cos(C) + k * sin(A) * cos(C) -
                   j * sin(A) * sin(B) * sin(C) + k * cos(A) * sin(B) * sin(C) -
                   i * cos(B) * sin(C);
        }

        double calculateZ(double i, double j, double k)
        {
            return k * cos(A) * cos(B) - j * sin(A) * cos(B) + i * sin(B);
        }

        void calculateForSurface(double cubeX, double cubeY, double cubeZ, double ch)
        {
            x = calculateX(cubeX, cubeY, cubeZ);
            y = calculateY(cubeX, cubeY, cubeZ);
            z = calculateZ(cubeX, cubeY, cubeZ) + distanceFromCam;

            ooz = 1 / z;

            xp = (int)(width / 2 + horizontalOffset + K1 * ooz * x * 2);
            yp = (int)(height / 2 + K1 * ooz * y);

            idx = xp + yp * width;
            if (idx >= 0 && idx < width * height)
            {
                if (ooz > zBuffer[idx])
                {
                    zBuffer[idx] = ooz;
                    buffer[idx] = ch;
                }
            }

        }
        static void Memset<T>(T[] array, T elem)
        {
            int length = array.Length;
            if (length == 0) return;
            array[0] = elem;
            int count;
            for (count = 1; count <= length / 2; count *= 2)
                Array.Copy(array, 0, array, count, count);
            Array.Copy(array, 0, array, count, length - count);
        }


        void main()
        {
            
            while (true)
            {
                /*Util.Memset(buffer, backgroundASCIICode, width * height);
                Util.Memset(zBuffer, 0, width * height * 4);*/
                Memset(buffer, backgroundASCIICode);
                Memset(zBuffer, 0);

                cubeWidth = 20;
                horizontalOffset = -2 * cubeWidth;
                // first cube
                for (double cubeX = -cubeWidth; cubeX < cubeWidth; cubeX += incrementSpeed)
                {
                    for (double cubeY = -cubeWidth; cubeY < cubeWidth;
                         cubeY += incrementSpeed)
                    {
                        calculateForSurface(cubeX, cubeY, -cubeWidth, 'п');
                        calculateForSurface(cubeWidth, cubeY, cubeX, 'и');
                        calculateForSurface(-cubeWidth, cubeY, -cubeX, 'в');
                        calculateForSurface(-cubeX, cubeY, cubeWidth, 'о');
                        calculateForSurface(cubeX, -cubeWidth, -cubeY, ';');
                        calculateForSurface(cubeX, cubeWidth, cubeY, '+');
                    }
                }
                Console.Clear();
                for (int k = 0; k < width * height; k++)
                {
                    //putchar(k % width ? buffer[k] : 10);
                    if (k % width != 0)
                    {
                        Console.Write((char)buffer[k]);
                    }
                    else
                    {
                        Console.Write((char)10);

                    }
                   
                }
                A += 0.05;
                B += 0.05;
                C += 0.01;

            }

        }
        main();
    }
}