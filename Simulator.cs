using System;
using System.IO.Ports;

class OBD2Simulator
{
    static void Main()
    {
        string portName = "COM3";
        SerialPort port = new(portName, 9600, Parity.None, 8, StopBits.One);

        try
        {
            port.Open();
            Console.WriteLine($"OBD2 szimulátor fut a {portName} porton...");

            while (true)
            {
                if (port.BytesToRead > 0)
                {
                    string? request = port.ReadExisting()?.Trim();
                    Console.WriteLine($"[DEBUG] Kapott OBD2 parancs: {request}");

                    string response = string.Empty;

                    switch (request)
                    {
                        case "010C":
                            response = "41 0C 1A F8";
                            break;
                        case "0100":
                            response = "41 00 BE 3F B8 13";
                            break;
                        case "010F":
                            response = "41 0F 5A";
                            break;
                        case "0111":
                            response = "41 11 2D";
                            break;
                        case "0105":
                            response = "41 05 1A";
                            break;
                        case "012F":
                            response = "41 2F 01";
                            break;
                        case "03":
                            response = "43 04 04 20 C4 17 40 70 97 93";
                            /*
                            43 04 04 20 C4 17 40 70 97 93:                          
                            | P0420 |
                            | U0417 |
                            | C0070 |
                            | B1793 |
                            
                            43 04 C1 00 45 62 92 34 03 00
                            | U0100 |
                            | C0562 |
                            | B1234 |
                            | P0300 |
                            */
                            break;
                        default:
                            response = "NO DATA";
                            break;
                    }                    

                    Console.WriteLine($"[DEBUG] Válasz küldése: {response}");
                    port.WriteLine(response);
                }

                Thread.Sleep(100);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Hiba: {ex.Message}");
        }
        finally
        {
            port.Close();
        }
    }
}