---
name: Program exception
about: Noticed an internal error? Please tell us more!
title: 'Program exception'
labels: bug, good first issue
assignees: CodingWonders

---

If you have run into an internal error (program exception), we would like to learn more about it. When an error occurs, its information will be copied to your clipboard. If you can't find error information on your clipboard, a local copy was also saved at `<program directory>\Logs\Errors`.

**PLEASE PROVIDE DETAILS ABOUT THE EXCEPTION, OR YOUR ISSUE WILL BE CLOSED WITHIN 4 HOURS**

**Exception information**: please tell us what happened below:

<!-- Exception Information. If you have the local copy on hand, please attach it -->

**How did it happen?** What steps did you perform in order to experience this problem?

<!-- Steps -->

### Attaching DynaLog event logs
---

DISMTools 0.6.1 and later use DynaLog as a means to write diagnostic information to a log file that you can send to the developers. The log is stored in `<program directory>\Logs`:

- If you are using an installed copy, go to `\Program Files\DISMTools\logs` for stable builds or `\Program Files\DISMTools\Preview\logs` for preview builds
- If you are using a portable copy, go to `<startup location>\logs`

The file in question is `DT_DynaLog.log`. Attach this log by dropping it below:

