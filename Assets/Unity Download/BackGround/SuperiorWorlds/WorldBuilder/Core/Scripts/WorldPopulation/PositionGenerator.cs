using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SuperiorWorlds
{
    public static class PositionGenerator
    {
        public static List<Vector2> FilterPoints(Texture2D heatmapTexture, Vector2 bottomLeft, Vector2 topRight, float minimumDistance, float minimumThreshold, float maximumThreshold, bool flipXAxis = false, bool flipZAxis = false, float rotationAngle = 0f)
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();

            List<Vector2> samplePoints = FastPoissonDiskSampling.Sampling(bottomLeft, topRight, minimumDistance);
            List<Color> colors = new List<Color>(samplePoints.Count); // Store the colors of the sample points

            // Get the colors from the heatmap texture in the main thread
            foreach (Vector2 point in samplePoints)
            {
                var pixelUV = new Vector2((point.x - bottomLeft.x) / (topRight.x - bottomLeft.x), (point.y - bottomLeft.y) / (topRight.y - bottomLeft.y));
                colors.Add(heatmapTexture.GetPixelBilinear(pixelUV.x, pixelUV.y));
            }

            stopwatch1.Stop();
            // UnityEngine.Debug.Log("FastPoissonDiskSampling processing time: " + stopwatch1.Elapsed.TotalMilliseconds.ToString("0.00") + " ms");

            List<Vector2> filteredPoints = new List<Vector2>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Parallel.For(0, samplePoints.Count, index =>
            {
                var point = samplePoints[index];
                var color = colors[index];

                // Check if the grayscale value of the color is within the specified range
                if (color.grayscale >= minimumThreshold && color.grayscale <= maximumThreshold)
                {
                    // Apply flipping and rotation transformations to the point coordinates
                    Vector2 transformedPoint = point;
                    if (flipXAxis)
                    {
                        transformedPoint.x = topRight.x - (point.x - bottomLeft.x);
                    }

                    if (flipZAxis)
                    {
                        transformedPoint.y = topRight.y - (point.y - bottomLeft.y);
                    }

                    if (rotationAngle != 0f)
                    {
                        float radians = rotationAngle * Mathf.Deg2Rad;
                        float cos = Mathf.Cos(radians);
                        float sin = Mathf.Sin(radians);

                        float xFromCenter = transformedPoint.x - (topRight.x + bottomLeft.x) * 0.5f;
                        float yFromCenter = transformedPoint.y - (topRight.y + bottomLeft.y) * 0.5f;
                        transformedPoint.x = Mathf.RoundToInt(xFromCenter * cos - yFromCenter * sin + (topRight.x + bottomLeft.x) * 0.5f);
                        transformedPoint.y = Mathf.RoundToInt(xFromCenter * sin + yFromCenter * cos + (topRight.y + bottomLeft.y) * 0.5f);
                    }

                    lock (filteredPoints) // Synchronize access to the filteredPoints list
                    {
                        filteredPoints.Add(transformedPoint);
                    }
                }
            });

            stopwatch.Stop();
            // UnityEngine.Debug.Log("Filter processing time: " + stopwatch.Elapsed.TotalMilliseconds.ToString("0.00") + " ms");

            return filteredPoints;
        }
    }
}
