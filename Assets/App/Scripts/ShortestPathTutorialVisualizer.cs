using System.Collections.Generic;
using UnityEngine;

public class ShortestPathTutorialVisualizer : MonoBehaviour
{

    #region Constants

    /// <summary>
    /// The color of the straight line crossing the sphere to connect the start and end points.
    /// </summary>
    private static readonly Color STRAIGHT_LINE_COLOR = Color.magenta;

    /// <summary>
    /// The color of the lines connecting the points on the straight line to the center of the sphere.
    /// </summary>
    private static readonly Color CONNECTING_LINE_COLOR = Color.magenta;

    /// <summary>
    /// The color of the lines projecting the points from the straight line to the sphere.
    /// </summary>
    private static readonly Color PROJECTION_LINE_COLOR = Color.magenta;

    /// <summary>
    /// The color of the shortest path.
    /// </summary>
    private static readonly Color SHORTEST_PATH_COLOR = Color.red;

    #endregion

    #region Fields/Properties

    /// <summary>
    /// References the scriptable object that stores the demo information.
    /// </summary>
    [Tooltip("References the scriptable object that stores the demo information.")]
    [SerializeField]
    private PathManagerScriptableObject PathsScriptableObject;

    /// <summary>
    /// References the sphere in the scene.
    /// </summary>
    public SphericalPaths.Sphere Sphere;

    /// <summary>
    /// Stores the points on the straight line.
    /// </summary>
    private List<Vector3> PointsOnTheStraightLine;

    /// <summary>
    /// Stores the projected points on the sphere.
    /// </summary>
    private List<Vector3> PointsOnTheSphere;

    #endregion

    #region Methods

    /// <summary>
    /// Visualizes the tutorial based on the provided step.
    /// </summary>
    public void Display(int step)
    {
        // Clear all paths
        Sphere.ClearPaths();

        // Display steps
        if (step >= 1)
            DisplayStep1();
        if (step >= 2)
            DisplayStep2();
        if (step >= 3)
            DisplayStep3();
        if (step >= 4)
            DisplayStep4();
    }

    /// <summary>
    /// Displays the first step in the tutorial.
    /// </summary>
    private void DisplayStep1()
    {
        // TODO: Display the center of the sphere

        // Display straight line crossing the sphere between the start and end points
        Sphere.DisplayPaths
        (
            new SphericalPaths.DataStructure.Path
            (
                new List<SphericalPaths.DataStructure.Coordinates>
                {
                    PathsScriptableObject.StartCoordinates,
                    PathsScriptableObject.EndCoordinates
                }
            ),
            STRAIGHT_LINE_COLOR
        );
    }

    /// <summary>
    /// Displays the second step in the tutorial.
    /// </summary>
    private void DisplayStep2()
    {
        // Compute the points on the straight line
        PointsOnTheStraightLine = SphericalPaths.DataStructure.PathComputationMethods.GetStraightLine
        (
            PathsScriptableObject.StartCoordinates.SphericalCoordinates,
            PathsScriptableObject.EndCoordinates.SphericalCoordinates,
            15
        );

        // TODO: Display each point on the straight line

        // Connect the points to the center of the sphere
        List<SphericalPaths.DataStructure.Path> paths = new List<SphericalPaths.DataStructure.Path>();
        foreach (var point in PointsOnTheStraightLine)
            paths.Add(new SphericalPaths.DataStructure.Path
            (
                new List<SphericalPaths.DataStructure.Coordinates>
                {
                    new SphericalPaths.DataStructure.Coordinates(point, PathsScriptableObject.StartCoordinates.Radius, PathsScriptableObject.StartCoordinates.Width),
                    new SphericalPaths.DataStructure.Coordinates(Vector3.zero, PathsScriptableObject.StartCoordinates.Radius, PathsScriptableObject.StartCoordinates.Width)
                }
            ));
        paths.Add(new SphericalPaths.DataStructure.Path
        (
            new List<SphericalPaths.DataStructure.Coordinates>
            {
                PathsScriptableObject.StartCoordinates,
                new SphericalPaths.DataStructure.Coordinates(Vector3.zero, PathsScriptableObject.StartCoordinates.Radius, PathsScriptableObject.StartCoordinates.Width)
            }
        ));
        paths.Add(new SphericalPaths.DataStructure.Path
        (
            new List<SphericalPaths.DataStructure.Coordinates>
            {
                PathsScriptableObject.EndCoordinates,
                new SphericalPaths.DataStructure.Coordinates(Vector3.zero, PathsScriptableObject.StartCoordinates.Radius, PathsScriptableObject.StartCoordinates.Width)
            }
        ));
        Sphere.DisplayPaths(paths, CONNECTING_LINE_COLOR);
    }

    /// <summary>
    /// Displays the third step in the tutorial.
    /// </summary>
    private void DisplayStep3()
    {
        // Compute the points on the arc
        PointsOnTheSphere = SphericalPaths.DataStructure.PathComputationMethods.ProjectPointsOnSphere
        (
            PointsOnTheStraightLine,
            PathsScriptableObject.StartCoordinates.Radius
        );

        // Connect the points on the straight line to their projection
        List<SphericalPaths.DataStructure.Path> paths = new List<SphericalPaths.DataStructure.Path>();
        for (int i = 0; i < PointsOnTheStraightLine.Count; i++)
            paths.Add(new SphericalPaths.DataStructure.Path
            (
                new List<SphericalPaths.DataStructure.Coordinates>
                {
                    new SphericalPaths.DataStructure.Coordinates(PointsOnTheStraightLine[i], PathsScriptableObject.StartCoordinates.Radius, PathsScriptableObject.StartCoordinates.Width),
                    new SphericalPaths.DataStructure.Coordinates(PointsOnTheSphere[i], PathsScriptableObject.StartCoordinates.Radius, PathsScriptableObject.StartCoordinates.Width)
                }
            )); 
        Sphere.DisplayPaths(paths, PROJECTION_LINE_COLOR);

        // TODO: Display spheres to represent the projected points
    }

    /// <summary>
    /// Displays the fourth step in the tutorial.
    /// </summary>
    private void DisplayStep4()
    {
        Sphere.DisplayPaths(PathsScriptableObject.ShortestPath, SHORTEST_PATH_COLOR);
    }

    #endregion

}
