import 'package:flutter_test/flutter_test.dart';
import 'package:study_planner/main.dart';

void main() {
  testWidgets('Timer page smoke test', (WidgetTester tester) async {
    // Build our app and trigger a frame.
    await tester.pumpWidget(const StudyPlannerApp());

    // Verify that our timer starts at 25:00.
    expect(find.text('25:00'), findsOneWidget);

    // Verify that the 'My Tasks' section is present.
    expect(find.text('My Tasks'), findsOneWidget);
  });
}
