<p align="center">
    <img src="resources/Image.png">
</p>

# ICE Instrument Monitor
The project was developed using .NET 6.0 and Visual Studio 2022. Issues may occur on older environments.

# Getting Started
1. **Quick Start Method**  
For a quick start, utilize Help > Execute Debug Mode. This will open a pre-configured setup that demonstrates the functionality of the front end.

2. **User Method**  
Alternatively, click Edit > Sources (meant to simulate configuration of an API linkage). Then, double-click on the left-most panel or use File > Add Ticker to open the `Add Symbol` window. Data will then be generated and displayed in the main panel.

3. **Unit Tests**  
Unit tests are found in the `SimulationEngineTests` project. These are used to test and demonstrate functionality that is not easily demonstrated in the front end.

# Features
- Real-time data display with dynamic adjustments
- Adding multiple instruments with different sources (ie: NASDAQ, NYSE, etc.)
- Alphabetical ordering of instruments
- Multi-threaded data generation
- Flawless window form resizing
- Quick start method for demonstration purposes

# Dependencies
- [ScottPlot](https://scottplot.net/)

# Assumptions
- The limited time frame of this project suggests a working application over a fully fleshed out one. Thus, the application is not meant to be an enterprise-ready codebase, but rather a proof of concept. Further, unit tests, documentation, and other best practices are far from polished.
- The application would be accepting of multiple data sources and would be able to handle a large number of symbols. Each source would be capable of running on its own thread and would be able to be paused or stopped at any time.

# Application Limitations
- A static one-day view is implemented using NYSE operations hours.
- Data Sources have no manual configuration in the demo.
- Data generation is scheduled to run every 500ms to speed up simulatation of a live feed rather than real-time.
- Charts have different y-intervals due to relative scaling of prices.
