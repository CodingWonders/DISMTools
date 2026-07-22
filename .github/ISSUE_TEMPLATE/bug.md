---
name: Bug
about: Noticed a bug? Let us know so we can fix it
title: ''
labels: bug, good first issue
assignees: CodingWonders

---

**What is the bug about?**
Please describe the bug you're experiencing. Examples:

- *When doing something, the program crashes*

**IF YOU HAVE ENCOUNTERED A PROGRAM EXCEPTION, YOU NEED TO USE ITS RESPECTIVE TEMPLATE. Go to the issue types and select "Program exception"**

**How does it happen?**
Please provide the steps necessary to reproduce this bug:



**Expected behavior**
A clear and concise description of what you expected to happen.

**Screenshots**
If applicable, add screenshots to help explain your problem.



**Additional context**
Add any other context about the problem here.

### Attaching DynaLog event logs
---

DISMTools 0.6.1 and later use DynaLog as a means to write diagnostic information to a log file that you can send to the developers. The log is stored in `<program directory>\Logs`:

- If you are using an installed copy, go to `\Program Files\DISMTools\logs` for stable builds or `\Program Files\DISMTools\Preview\logs` for preview builds
- If you are using a portable copy, go to `<startup location>\logs`

The file in question is `DT_DynaLog.log`. Attach this log by dropping it below:

