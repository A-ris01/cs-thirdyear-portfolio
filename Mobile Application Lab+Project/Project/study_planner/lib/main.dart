import 'package:flutter/material.dart';
import 'dart:async';

// --- CONSTANTS ---
const kActiveCardColor = Color(0xFF434445);
const kInactiveCardColor = Color(0xFFDCDCDC);
const kBottomContainerColor = Color(0xFF2196F3);
const kDarkBlueBg = Color(0xFF0A1930);
const kDangerColor = Color(0xFFE53935);

const kLabelTextStyle = TextStyle(
  fontSize: 18.0,
  fontWeight: FontWeight.bold,
  color: Colors.black87,
);

const kNumberTextStyle = TextStyle(
  fontSize: 60.0,
  fontWeight: FontWeight.w900,
  color: Colors.black87,
);

const kLargeButtonTextStyle = TextStyle(
  fontSize: 18.0,
  fontWeight: FontWeight.bold,
  color: Colors.white,
);

void main() {
  runApp(const StudyPlannerApp());
}

class StudyPlannerApp extends StatelessWidget {
  const StudyPlannerApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Study Planner & Pomodoro',
      theme: ThemeData.light().copyWith(
        primaryColor: kDarkBlueBg,
        scaffoldBackgroundColor: Colors.white,
        appBarTheme: const AppBarTheme(
          backgroundColor: kDarkBlueBg,
          centerTitle: true,
          titleTextStyle: kLargeButtonTextStyle,
          iconTheme: IconThemeData(color: Colors.white),
        ),
      ),
      home: const TimerPage(title: 'Study Planner'),
      debugShowCheckedModeBanner: false,
    );
  }
}

// --- MAIN SCREEN ---
class TimerPage extends StatefulWidget {
  const TimerPage({super.key, required this.title});
  final String title;

  @override
  State<TimerPage> createState() => _TimerPageState();
}

class _TimerPageState extends State<TimerPage> {
  static const int studyTimeInMinutes = 25;
  static const int breakTimeInMinutes = 5;

  int _secondsRemaining = studyTimeInMinutes * 60;
  bool _isStudyMode = true;
  bool _isRunning = false;
  Timer? _timer;

  final List<String> _tasks = [];
  final TextEditingController _taskController = TextEditingController();

  void _startTimer() {
    if (_timer != null && _timer!.isActive) return;
    
    setState(() {
      _isRunning = true;
    });

    _timer = Timer.periodic(const Duration(seconds: 1), (timer) {
      if (_secondsRemaining > 0) {
        setState(() {
          _secondsRemaining--;
        });
      } else {
        _stopTimer();
        _switchMode(); 
      }
    });
  }

  void _stopTimer() {
    if (_timer != null) {
      _timer!.cancel();
    }
    setState(() {
      _isRunning = false;
    });
  }

  void _resetTimer() {
    _stopTimer();
    setState(() {
      _secondsRemaining = (_isStudyMode ? studyTimeInMinutes : breakTimeInMinutes) * 60;
    });
  }

  void _switchMode() {
    _stopTimer();
    setState(() {
      _isStudyMode = !_isStudyMode;
      _secondsRemaining = (_isStudyMode ? studyTimeInMinutes : breakTimeInMinutes) * 60;
    });
  }

  void _addTask() {
    if (_taskController.text.trim().isNotEmpty) {
      setState(() {
        _tasks.add(_taskController.text.trim());
        _taskController.clear();
      });
    }
  }

  void _removeTask(int index) {
    setState(() {
      _tasks.removeAt(index);
    });
  }

  String _formatTime(int seconds) {
    int m = seconds ~/ 60;
    int s = seconds % 60;
    return '${m.toString().padLeft(2, '0')}:${s.toString().padLeft(2, '0')}';
  }

  @override
  void dispose() {
    _timer?.cancel();
    _taskController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          // Mode Indicator
          Padding(
            padding: const EdgeInsets.only(top: 15.0),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  _isStudyMode ? ' STUDY TIME' : ' BREAK TIME',
                  style: TextStyle(
                    fontSize: 22.0,
                    fontWeight: FontWeight.bold,
                    color: _isStudyMode ? kDarkBlueBg : kDangerColor,
                  ),
                ),
              ],
            ),
          ),
          
          // Timer Card
          Expanded(
            flex: 2,
            child: ReusableCard(
              color: kInactiveCardColor,
              cardChild: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  Text(_formatTime(_secondsRemaining), style: kNumberTextStyle),
                  const SizedBox(height: 10),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      RoundIconButton(
                        icon: _isRunning ? Icons.pause : Icons.play_arrow,
                        onPressed: _isRunning ? _stopTimer : _startTimer,
                      ),
                      const SizedBox(width: 20.0),
                      RoundIconButton(
                        icon: Icons.refresh,
                        onPressed: _resetTimer,
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
          
          // Task Management Header
          const Padding(
            padding: EdgeInsets.symmetric(horizontal: 15.0, vertical: 5.0),
            child: Text('My Tasks', style: kLabelTextStyle),
          ),

          // Add Task Input
          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 10.0),
            child: Row(
              children: [
                Expanded(
                  child: TextField(
                    controller: _taskController,
                    decoration: const InputDecoration(
                      hintText: 'Enter a task or subject...',
                      border: OutlineInputBorder(),
                      contentPadding: EdgeInsets.symmetric(horizontal: 15.0),
                    ),
                    onSubmitted: (_) => _addTask(),
                  ),
                ),
                const SizedBox(width: 10),
                RoundIconButton(
                  icon: Icons.add,
                  onPressed: _addTask,
                )
              ],
            ),
          ),
          
          // Task List
          Expanded(
            flex: 3,
            child: ReusableCard(
              color: Colors.white,
              cardChild: _tasks.isEmpty
                  ? const Center(child: Text('No tasks added yet.', style: TextStyle(color: Colors.grey)))
                  : ListView.builder(
                      itemCount: _tasks.length,
                      itemBuilder: (context, index) {
                        return ListTile(
                          leading: const Icon(Icons.check_circle_outline, color: kDarkBlueBg),
                          title: Text(_tasks[index], style: const TextStyle(fontSize: 16.0, fontWeight: FontWeight.w500)),
                          trailing: IconButton(
                            icon: const Icon(Icons.delete, color: Colors.grey),
                            onPressed: () => _removeTask(index),
                          ),
                        );
                      },
                    ),
            ),
          ),

          // Bottom Switch Button
          BottomButton(
            buttonTitle: _isStudyMode ? 'SWITCH TO BREAK' : 'SWITCH TO STUDY',
            color: _isStudyMode ? kBottomContainerColor : kDangerColor,
            onTap: _switchMode,
          ),
        ],
      ),
    );
  }
}


// --- CUSTOM WIDGET COMPONENTS ---

class ReusableCard extends StatelessWidget {
  const ReusableCard({super.key, required this.color, this.cardChild, this.onPress});

  final Color color;
  final Widget? cardChild;
  final VoidCallback? onPress;

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onPress,
      child: Container(
        margin: const EdgeInsets.all(10.0),
        decoration: BoxDecoration(color: color, borderRadius: BorderRadius.circular(10.0)),
        child: cardChild,
      ),
    );
  }
}

class RoundIconButton extends StatelessWidget {
  const RoundIconButton({super.key, required this.icon, required this.onPressed});

  final IconData icon;
  final VoidCallback onPressed;

  @override
  Widget build(BuildContext context) {
    return RawMaterialButton(
      onPressed: onPressed,
      elevation: 2.0,
      constraints: const BoxConstraints.tightFor(width: 45.0, height: 45.0),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10.0)),
      fillColor: const Color(0xFFE8E8FF),
      child: Icon(icon, color: Colors.black87),
    );
  }
}

class BottomButton extends StatelessWidget {
  const BottomButton({super.key, required this.onTap, required this.buttonTitle, this.color = kBottomContainerColor});

  final VoidCallback onTap;
  final String buttonTitle;
  final Color color;

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: onTap,
      child: Container(
        color: color,
        margin: const EdgeInsets.only(top: 10.0),
        width: double.infinity,
        height: 60.0,
        child: Center(child: Text(buttonTitle, style: kLargeButtonTextStyle)),
      ),
    );
  }
}
