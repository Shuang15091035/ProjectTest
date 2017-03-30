//
//  bubbleSort.m
//  DataStruct
//
//  Created by mac zdszkj on 2017/3/27.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "sort.h"


/**
 冒泡排序（每趟排序两两比较，找出最大的一个，比较n-1趟）

 @param array 数组
 @param length 数组长度
 */
void bubble_sort (int array[], int length){
    int temp = 0;
    for (int i = 0; i < length - 1; i++) {
        for (int j = 0; j < length - 1 - i; j++) {
            if (array[j] < array[j+1]) {
                temp = array[j+1];
                array[j+1] = array[j];
                array[j] = temp;
            }
        }
    }
}

void simpleSelectSorting (int array[], int length){
    int index = 0, temp = 0;
    for (int i = 0; i < length-1; i++) {
        index = i;
        for (int j = i + 1; j < length; j++) {
            if (array[index] > array[j]) {
                index = j;
            }
        }
        if (index != i) {
            temp = array[i];
            array[i] = array[index];
            array[index] = temp;
        }
    }
}


/**
 直接插入排序（将一个记录插入到已排序好的有序表中，从而得到一个新，记录数增1的有序表。即：先将序列的第1个记录看成是一个有序的子序列，然后从第2个记录逐个进行插入，直至整个序列有序为止）

 @param array 数组
 @param length 数组长度
 */
void straightInsertSort (int array[], int length){
    int temp = 0, index = 0;
    for (int i = 1; i < length; i++) {
        if (array[i] < array[i-1]) {
            index = i-1;
            temp = array[i];
            while (temp < array[index] && index > -1) {
                array[index+1] = array[index];
                index--;
            }
            array[index+1] = temp;
        }
    }
}

/**
 希尔排序方法/缩小增量排序（选取一个增量因子，增量因子中除1 外没有公因子，且最后一个增量因子必须为1。）//利用增量因子拆分成若干组子序列，各子表进行直接插入排序

 @param array 数组
 @param length 数组长度
 */
void insertSortOrShellSort (int array[], int length){
    int dt = length/2;
    while (dt >= 1) {
        for(int i = dt; i< length; i++){
            if (array[i] < array[i-dt]) {
                int index = i-dt;
                int temp = array[i-dt];
                while (temp < array[index] && index > -1) {
                    array[index+dt] = array[index];
                    index -= dt;
                }
                array[index+dt] = temp;
            }
        }
        dt = dt/2;
    }
}
