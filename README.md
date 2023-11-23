# Private-repo
WPS APPLICATION SprayPaint 

The Spray Paint application is a Windows Presentation Foundation (WPF) tool developed to provide users with an interactive platform for spray painting on images. The application supports features such as image loading, spray painting, color and density adjustments, erasing, and the ability to save changes. This documentation provides a detailed overview of the application's key features, implementation details, challenges faced, and solutions employed.

Key Features
1. Image Loading:
Users can load images of various formats onto the canvas using the "Select Image" button. The application dynamically repositions the loaded image to ensure proper centering and visibility within the canvas. This feature enhances user experience, especially when dealing with images of varying dimensions.
Implementation:
Utilized Microsoft.Win32.OpenFileDialog for image selection.
Implemented a dynamic recentering mechanism to calculate and apply margins for proper image placement.
Considered maintaining the aspect ratio of the image to ensure a visually appealing display.

2. Spray Painting:
Interactive spray painting is achieved through mouse events (MouseDown, MouseUp, MouseMove) on the canvas. The application creates ellipses with varying colors and densities, simulating a spray paint effect. Users can control the color and density of the spray paint.
Implementation:
Utilized mouse event handlers to detect user interactions on the canvas.
Created ellipses dynamically based on user-selected color and density using the brush size slider.

3. Color and Density Adjustment:
Users can customize paint properties by selecting colors from a dropdown color picker and adjusting density using a slider. This allows for a diverse range of creative expression.
Implementation:
Integrated a ComboBox for color selection and a Slider for adjusting the brush size (density).
Implemented logic to apply the selected color and density to the spray paint strokes.

4. Erasing:
The eraser tool enables users to selectively remove sprayed shapes from the canvas. This presented a challenge in accurately identifying and removing sprayed shapes.
Implementation:
Implemented a toggle mechanism for the eraser using the "Eraser" button.
Developed a mechanism to check for intersections between the eraser and sprayed shapes, ensuring accurate removal.

5. Save Changes:
Users can save the modified image, overwriting the original file. This feature provides a tangible outcome of the creative process.
Implementation:
Utilized Microsoft.Win32.SaveFileDialog for saving the modified image.
Implemented the SaveChangesToFile method to persist changes to the original image file.

6. Session Persistence:
Users can save spray paint sessions separately from the original image, allowing for later modifications without altering the base image.
Implementation:
Developed a mechanism to save and load spray paint sessions as separate ".spray" files.
Ensured that the original image remains unaltered during session persistence.

Challenges and Solutions
1. Eraser Functionality:
Challenge: Implementing an effective eraser tool.
Solution: Developed a mechanism to identify and remove sprayed shapes accurately. This involved checking for intersections between the eraser and sprayed shapes and removing the intersecting shapes.

2. Session Persistence:
Challenge: Managing session persistence without modifying the original image.
Solution: Implemented separate saving and loading mechanisms for spray paint sessions. This ensured that the original image remains intact while allowing users to resume their sessions.

3. Image Recentering:
Challenge: Properly recentering loaded images within the canvas.
Solution: Dynamically calculated margins to center images while maintaining aspect ratios. This addressed issues where parts of the image were outside the visible area.
