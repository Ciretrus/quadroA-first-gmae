using System.Collections.Generic;
using UnityEngine;

public static class UnistrokeRecognizer
{
    private const int m_interpolationDotsCount = 64;

    public static List<Vector3> GetNormalizedPoints(List<Vector3> points) 
    {
        points = Resample(points, m_interpolationDotsCount);
        points = ScaleToSquare(points);
        points = TranslateToOrigin(points);

        return new List<Vector3>(points);
    }

    public static float GetDistanceBetweenDraws(List<Vector3> originalDots, List<Vector3> newDots)
    {
        float sum = 0;

        for (int i = 0; i < originalDots.Count; i++)
        {   
            sum += Mathf.Abs(originalDots[i].magnitude - newDots[i].magnitude);
        }

        return sum;
    }

    private static List<Vector3> Resample(List<Vector3> points, int n)
    {
        float segmentLength = GetPathLength(points) / (n - 1); 
        float sumLength = 0f;

        List<Vector3> newPoints = new List<Vector3> { points[0] };

        for (int i = 1; i < points.Count; i++)
        {            
            Vector3 previousPoint = points[i - 1];
            Vector3 currentPoint = points[i];
            float currentSegmentLength = Vector3.Distance(previousPoint, currentPoint);

            while (sumLength + currentSegmentLength >= segmentLength)
            {
                float t = (segmentLength - sumLength) / currentSegmentLength;
                Vector3 newPoint = Vector3.Lerp(previousPoint, currentPoint, t);
                newPoints.Add(newPoint);

                previousPoint = newPoint;
                currentSegmentLength = Vector3.Distance(previousPoint, currentPoint);
                sumLength = 0;
            }

            sumLength += currentSegmentLength;
        }

        if (newPoints.Count < n)
            newPoints.Add(points[points.Count - 1]);

        return newPoints;
    }

    private static List<Vector3> ScaleToSquare(List<Vector3> points)
    {
        float maxWidth = points[0].x;
        float maxHeight = points[0].y;
        float minWidth = points[0].x;
        float minHeight = points[0].y;

        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].x > maxWidth) maxWidth = points[i].x;
            if (points[i].y > maxHeight) maxHeight = points[i].y;
            if (points[i].x < minWidth) minWidth = points[i].x;
            if (points[i].y < minHeight) minHeight = points[i].y;
        }

        float width = maxWidth - minWidth;
        float height = maxHeight - minHeight;

        float scale = Mathf.Max(height, width);

        for (int i = 0; i < points.Count; i++)
        {
            points[i] *= scale;
        }

        return points;
    }

    private static List<Vector3> TranslateToOrigin(List<Vector3> points)
    {
        Vector3 center = GetCentroid(points);
        for (int i = 0; i < points.Count; i++)
        {
            points[i] -= center;
        }

        return points;
    }

    private static Vector3 GetCentroid(List<Vector3> points)
    {
        Vector3 sum = Vector3.zero;
        foreach (var point in points)
            sum += point;

        return sum / points.Count;
    }

    private static float GetPathLength(List<Vector3> points)
    {
        float length = 0f;

        for (int i = 1; i < points.Count; i++)
        {
            length += Vector3.Distance(points[i - 1], points[i]);
        }

        return length;
    }

    private static List<Vector3> RotateBy(List<Vector3> points, float angle)
    {
        Vector3 centroid = GetCentroid(points);
        List<Vector3> rotated = new List<Vector3>();

        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);

        foreach (var p in points)
        {
            float dx = p.x - centroid.x;
            float dy = p.y - centroid.y;

            float newX = dx * cos - dy * sin + centroid.x;
            float newY = dx * sin + dy * cos + centroid.y;

            rotated.Add(new Vector3(newX, newY));
        }

        return rotated;
    }
}