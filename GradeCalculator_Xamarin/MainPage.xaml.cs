using System;
using Xamarin.Forms;

namespace GradeCalculator_Xamarin
{
	public partial class MainPage
	{
		private int _grade;
		private string _letter = "(??)";
		private string _result = "(??)";

		public MainPage()
		{
			InitializeComponent();
		}

		private void HomeworkPercentage_ValueChanged(object sender, ValueChangedEventArgs args)
		{
			//Calculate();
		}

		private void HomeworkPercentage_DragCompleted(object sender, EventArgs e)
		{
			Calculate();
		}

		private void FinalPercentage_ValueChanged(object sender, ValueChangedEventArgs args)
		{
			//Calculate();
		}

		private void FinalPercentage_DragCompleted(object sender, EventArgs e)
		{
			Calculate();
		}


		private void CalculateButton_Clicked(object sender, EventArgs e)
		{
			Calculate();
		}

		private void Calculate()
		{
			if (!ValidateInputs()) return;
			CalculateGrade();
			CalculateLetterAndResult();
			Display();
		}

		private bool ValidateInputs()
		{
			var errors = "";
			if (string.IsNullOrEmpty(MidtermScore.Text)) errors += "Vize";

			if (string.IsNullOrEmpty(FinalScore.Text)) errors += string.IsNullOrEmpty(errors) ? "Final" : ", Final";

			if (HomeworkPercentage.Value > 0 && string.IsNullOrEmpty(HomeworkScore.Text))
				errors += string.IsNullOrEmpty(errors) ? "Ödev" : ", Ödev";

			if (string.IsNullOrEmpty(errors)) return true;

			DisplayAlert("Formu eksiksiz doldurun!", $"{errors} puanı eksik!", "OK");
			ResetInputs();
			Display();
			return false;
		}

		private void CalculateGrade()
		{
			var homeworkScoreInt = 0;
			if (HomeworkPercentage.Value > 0 && !string.IsNullOrEmpty(HomeworkScore.Text))
				homeworkScoreInt = int.Parse(HomeworkScore.Text);

			var midtermScoreInt = int.Parse(MidtermScore.Text);
			var finalScoreInt = int.Parse(FinalScore.Text);

			var homeworkPercentageDouble = 0.0;
			if (HomeworkPercentage.Value > 0) homeworkPercentageDouble = HomeworkPercentage.Value / 100.0;

			var midtermPercentageDouble = 1.0;
			if (homeworkPercentageDouble > 0) midtermPercentageDouble -= homeworkPercentageDouble;

			var finalPercentageDouble = FinalPercentage.Value / 100.0;

			var homework = 0.0;
			if (homeworkPercentageDouble > 0) homework = homeworkScoreInt * homeworkPercentageDouble;

			var midterm = midtermScoreInt * midtermPercentageDouble;
			var homeworkMidterm = (homework + midterm) * (1 - finalPercentageDouble);
			var final = finalScoreInt * finalPercentageDouble;

			_grade = (int) (homeworkMidterm + final);
		}

		private void CalculateLetterAndResult()
		{
			switch (_grade)
			{
				case int n when n >= 88 && n <= 100:
					_letter = "AA";
					_result = "Mükemmel";
					break;

				case int n when n >= 80 && n <= 87:
					_letter = "BA";
					_result = "Çok İyi";
					break;

				case int n when n >= 73 && n <= 79:
					_letter = "BB";
					_result = "İyi";
					break;

				case int n when n >= 66 && n <= 72:
					_letter = "CB";
					_result = "Orta";
					break;

				case int n when n >= 60 && n <= 65:
					_letter = "CC";
					_result = "Yeterli";
					break;

				case int n when n >= 55 && n <= 59:
					_letter = "DC";
					_result = "Şartlı başarılı veya başarısız";
					break;

				case int n when n >= 50 && n <= 54:
					_letter = "DD";
					_result = "Şartlı başarılı veya başarısız";
					break;

				default:
					_letter = "FF";
					_result = "Sınavda başarısız";
					break;
			}
		}

		private void Display()
		{
			HbnLbl.Text = $"HBN: {_grade}";
			LetterLbl.Text = $"HARF NOTU: {_letter}";
			ResultLbl.Text = $"Açıklama: {_result}";
		}

		private void ResetInputs()
		{
			_grade = 0;
			_letter = "(??)";
			_result = "(??)";
		}
	}
}