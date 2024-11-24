# Team-Based Painting Simulation

## Project Overview
This project simulates a team-based painting task, optimized through parallel computing. Each worker independently paints circles, and the execution time is analyzed under varying degrees of parallelization.

---

## Program Structure

### Models
- **`Circle.cs`**: Defines circle attributes (ID, coordinates).
- **`Worker.cs`**: Manages worker-specific tasks and painted circle records.

### Services
- **`SimulationManager.cs`**: Centralized state management to prevent duplicate painting.

### Utilities
- **`CircleGenerator.cs`**: Randomly generates circle coordinates.

### Visualization
- **`MainWindow.xaml` & `MainWindow.xaml.cs`**:
  - **UI**: Displays the painting progress.
  - **Core Logic**: Assigns circles, orchestrates tasks, and updates the UI.



---

## Dependencies
- Exclusively uses `.NET` libraries and **Windows Presentation Foundation (WPF)**.
- No external dependencies.

---

## Evaluation of Parallelization

### Parallelization Feasibility
- **Yes**, as painting tasks are independent and allow for concurrent execution without inter-worker interference.

### Problem Partitioning
- **Approach**: Round-robin assignment (`circle ID % K`), ensuring equitable distribution of tasks among workers.

### Communication Requirements
- Minimal but necessary:
  - Workers interact with `SimulationManager` to validate painting status.

### Data Dependencies
- Low, relying solely on `SimulationManager` to prevent rework.

### Synchronization Needs
- Thread-safe mechanisms (e.g., `ConcurrentDictionary`) ensure proper management of shared resources.

### Load Balancing Concerns
- Minimal, due to:
  - Large circle counts that ensure near-even workloads.
  - Dynamic assignment strategies to handle minor imbalances.

---

## Test Results

### Execution Time Across Varying Workers
| Number of Workers (K) | Execution Time (ms) |
|------------------------|---------------------|
| 5                      | 4832                |
| 20                     | 1066                |
| 100                    | 239                 |

---

## Analysis
- **K = 5**: Limited parallelism results in significant delays.
- **K = 20**: Effective workload division greatly reduces execution time.
- **K = 100**: Peak performance achieved due to maximum parallelism. Further increases in `K` may yield diminishing returns due to system constraints (e.g., CPU cores, synchronization overhead).

---

## Conclusion
Increasing the number of workers (`K`) enhances efficiency, confirming the advantages of parallel computing. However:
- **Optimal Worker Count**: Should align with hardware capabilities to avoid thread-management overhead.

