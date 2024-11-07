using System;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Production_ex
{
    // 생산 관련 데이터 저장
    struct productinfo
    {
        public string item; // 생산품목
        public string workcenter; // 작업장
        public int qty; // 생산수량
        public bool complete; // 생산 완료 여부
    }
    class Program
    {
        // 특정일에 생산 계획건 지정
        static void ProductionPlan(productinfo[] p)
        {
            Console.WriteLine("날짜를 입력하세요(1~31)");
            int day = int.Parse(Console.ReadLine());
            if (0 < day && day < 32)
            {
                Console.Write("생산 품목을 입력하세요");
                p[day - 1].item = Console.ReadLine();
                Console.Write("작업장을 입력하세요");
                p[day - 1].workcenter = Console.ReadLine();
                Console.Write("생산량을 입력하세요");
                p[day - 1].qty = int.Parse(Console.ReadLine());
                p[day - 1].complete = false;
            }
            else
            {
                Console.WriteLine("잘못된 날짜 입력입니다");
            }
        }
        // 특정 생산건을 선택하여 생산완료된것을 지정
        static void CompleteProcess(productinfo[] p)
        {
            Console.Write("생산 계획이 입력된 날짜는");
            for (int i = 0; i < 31; i++)
            {
                if (p[i].qty > 0 && p[i].complete == false)
                {
                    Console.Write("{0},", i + 1);
                }
            }
            Console.WriteLine("입니다");
            Console.Write("생산 완료된 생산 계획건 날짜를 입력해 주세요");
            int day = int.Parse(Console.ReadLine());
            if (0 < day && day < 32)
            {
                p[day - 1].complete = true;
            }
            else
            {
                Console.WriteLine("잘못된 날짜입력입니다");
            }
        }
        //생산 계획량 완료량 진척률을 계산해서 보여준다
        static void ProductionSummary(productinfo[] p, int type)
        {
            Hashtable plan = new Hashtable(); // 항목별 계획량을 저장
            for (int i = 0; i < 31; i++)
            {
                string key;
                if (type == 3)
                {
                    key = p[i].item; // type 가 3인 경우 품목별 정리
                }
                else
                {
                    key = p[i].workcenter;
                }
                if (key == null)
                {
                    continue;
                }
                else if (plan.ContainsKey(key))
                {
                    plan[key] = (int)plan[key] + p[i].qty; // 기존값에 합산
                }
                else
                {
                    plan.Add(key, p[i].qty); // 수량 등록
                }
            }
            Hashtable complete = new Hashtable(); // 항목별 완료량 저장
            for (int i = 0; i < 31; i++)
            {
                string key;
                if (type == 3)
                {
                    key = p[i].item;
                }
                else
                {
                    key = p[i].workcenter;
                }
                if (key == null)
                {
                    continue;
                }
                else if (p[i].complete == false)
                {
                    continue;
                }
                else if (complete.ContainsKey(key))
                {
                    complete[key] = (int)complete[key] + p[i].qty; // 기존값에 합산
                }
                else
                {
                    complete.Add(key, p[i].qty); // 수량등록
                }
            }
            foreach (DictionaryEntry cs in plan)
            {
                string name = (string)cs.Key;
                int planQty = (int)cs.Value; // 계획량 가져오기
                int completeQty = 0;
                if (complete.ContainsKey(name))
                {
                    completeQty = (int)complete[name]; // 완료량이 있으면 가져오기
                }
                double rate = completeQty * 100.0 / planQty; // 진척률 계산 하기
                if (type == 3)
                {
                    Console.WriteLine("{0} 품목 생산 계획량 = {1}, 완료량 ={2}, 진척률 = {3,2:F}%", name, planQty, completeQty, rate);
                }
                else
                {
                    Console.WriteLine("{0} 작업장 생산 계획량 = {1}, 완료량 ={2}, 진척률 = {3,2:F}%", name, planQty, completeQty, rate);
                }
            }
        }
        static void Main(string[] args)
        {
            productinfo[] data = new productinfo[31];
            while (true)
            {
                Console.WriteLine("# 1 = 생산 계획 입력 2 = 생산 완료 입력 3 = 품목별 생산 실적 분석 4 = 작업장별 생산 실적 분석 0 = 프로그램 종료 #");
                Console.Write("원하는 작업을 입력하세요");
                int command = int.Parse(Console.ReadLine());
                if (command == 0)
                {
                    break;
                }
                switch (command)
                {
                    case 1:
                        ProductionPlan(data);
                        Console.WriteLine();
                        break;
                    case 2:
                        CompleteProcess(data);
                        Console.WriteLine();
                        break;
                    case 3:
                        ProductionSummary(data, command); // 3번일 경우 품목별 정리
                        Console.WriteLine();
                        break;
                    case 4:
                        ProductionSummary(data, command); // 4번일 경우 작업장별 정리
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
