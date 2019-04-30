#pragma once
typedef int(*MessageBoxCharlieCallback)(struct CHARLIE_BRAVO* charlieBravo);
void ShowMessageBoxCharlie(MessageBoxCharlieCallback messageBoxCharlieCallback);